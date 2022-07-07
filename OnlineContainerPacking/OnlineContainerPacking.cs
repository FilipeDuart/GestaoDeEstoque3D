using OnlineContainerPacking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OnlineContainerPacking
{
	/// <summary>
	/// A 3D bin packing algorithm originally ported from https://github.com/keremdemirer/3dbinpackingjs,
	/// which itself was a JavaScript port of https://github.com/wknechtel/3d-bin-pack/, which is a C reconstruction 
	/// of a novel algorithm developed in a U.S. Air Force master's thesis by Erhan Baltacioglu in 2001.
	/// </summary>
	public class OnlineContainerPacking
	{
		#region Private Variables

		private List<Item> itemsToPack;
		private List<Item> itemsPackedInOrder;

		private ScrapPad scrapfirst;

		private Container container;

		private decimal packedVolume;
		private decimal itemsToPackCount;
		private decimal totalItemVolume;

		#endregion Private Variables

		#region Private Methods

		/// <summary>
		/// Initializes everything.
		/// </summary>
		private void Initialize(Container container, List<Item> items)
		{
			itemsToPack = new List<Item>();
			itemsPackedInOrder = new List<Item>();

			// The original code uses 1-based indexing everywhere. This fake entry is added to the beginning
			// of the list to make that possible.
			itemsToPack.Add(new Item(0, 0, 0, 0, 0));

			itemsToPackCount = 0;

			foreach (Item item in container.Items)
			{
				if (item.IsPacked)
				{
					itemsPackedInOrder.Add(item);
				}
			}

			foreach (Item item in items)
			{
				for (int i = 1; i <= item.Quantity; i++)
				{
					//ORIGINAL
					//Item newItem = new Item(item.ID, item.Dim1, item.Dim2, item.Dim3, item.Quantity);
					//itemsToPack.Add(newItem);

					//ONLINE
					itemsToPack.Add(item);

					if (item.IsPacked)
					{
						itemsPackedInOrder.Add(item);
					}
				}

				itemsToPackCount += item.Quantity;
			}

			itemsToPack.Add(new Item(0, 0, 0, 0, 0));

			totalItemVolume = 0.0M;

			for (var i = 1; i <= itemsToPackCount; i++)
			{
				totalItemVolume = totalItemVolume + itemsToPack[i].Volume;
			}

			scrapfirst = new ScrapPad();

			scrapfirst.Pre = null;
			scrapfirst.Post = null;
		}

		/// <summary>
		/// Runs the packing algorithm.
		/// </summary>
		/// <param name="container">The container to pack items into.</param>
		/// <param name="items">The items to pack.</param>
		/// <returns>The bin packing result.</returns>
		public AlgorithmPackingResult OnlineRun(Container container, List<Item> items)
		{
			Initialize(container, items);
			OnlineReport(container);

			AlgorithmPackingResult result = new AlgorithmPackingResult();
			result.AlgorithmName = "EB-AFIT-LEAN";

			for (int i = 1; i <= itemsToPackCount; i++)
			{
				itemsToPack[i].Quantity = 1;

				if (!itemsToPack[i].IsPacked)
				{
					result.UnpackedItems.Add(itemsToPack[i]);
				}
			}

			result.PackedItems = itemsPackedInOrder;

			if (result.UnpackedItems.Count == 0)
			{
				result.IsCompletePack = true;
			}

			return result;
		}

		/// <summary>
		/// Using the parameters found, packs the best solution found and
		/// reports to the console.
		/// </summary>
		private void OnlineReport(Container container)
		{
			this.container = container;

			PackLayerInferiores();

			PackLayersSuperiores();
		}

		private void PackLayerInferiores()
		{
			foreach (var item in itemsToPack.Where(i => !i.IsPacked))
			{
				//Ignora itens se volume
				if (item.Volume == 0)
					continue;

				var itemsPacked = itemsPackedInOrder.Where(i => i.CoordY == 0).OrderBy(i => i.CoordZ);

				//Verifica se altura cabe
				if (item.Dim2 > container.Height)
					continue;

				//Se nao tiver nenhum item empacotado, ou nenhum empacotado no (0,0) entao tenta empacotar no (0,0)
				if (itemsPacked.Count() == 0 || itemsPacked.Where(i => i.CoordX == 0 && i.CoordZ == 0).FirstOrDefault() == null)
				{
					var itemNoGap = itemsPacked.Where(i =>
					i.CoordX > 0 &&
					i.CoordX < item.Dim1 &&
					i.CoordZ > 0 &&
					i.CoordZ < item.Dim3).FirstOrDefault();

					if (itemNoGap == null)
					{
						//Verificar se esta dentro dos limites do container
						if (item.Dim1 <= container.Length &&
							item.Dim3 <= container.Width)
						{
							//Empacota
							item.CoordX = 0;
							item.CoordY = 0;
							item.CoordZ = 0;

							item.PackDimX = item.Dim1;
							item.PackDimY = item.Dim2;
							item.PackDimZ = item.Dim3;

							item.ItemBaseId = null;
							item.IsPacked = true;

							packedVolume = packedVolume + item.Volume;
							itemsPackedInOrder.Add(item);

							//Verifica altura
							if (container.Height < item.Dim2 * (item.ItensEmpilhados.Count + 2))
							{
								item.EmpilhamentoDisponivel = false;
							}
							else
							{
								item.EmpilhamentoDisponivel = true;
							}

							//Verifica peso maximo
							if ((item.ItensEmpilhados.Count + 1) * item.Peso > item.PesoMaximo)
							{
								item.EmpilhamentoDisponivel = false;
							}
						}
					}
				}

				//Verifica se empacotou no (0,0)
				if (item.IsPacked)
					continue;

				//Tenta empacotar no sentido da direita
				foreach (var itemPacked in itemsPacked)
				{
					var itemNoGap = itemsPacked.Where(i => 
					i.CoordX >= itemPacked.CoordX + itemPacked.Dim1 && 
					i.CoordX < itemPacked.CoordX + itemPacked.Dim1 + item.Dim1 && 
					((i.CoordZ >= itemPacked.CoordZ &&
					i.CoordZ < itemPacked.CoordZ + item.Dim3) ||
					(itemPacked.CoordZ >= i.CoordZ &&
					itemPacked.CoordZ < i.CoordZ + i.Dim3))).FirstOrDefault();

					if(itemNoGap == null)
                    {
                        //Verificar se esta dentro dos limites do container
                        if (itemPacked.CoordX + itemPacked.Dim1 + item.Dim1 <= container.Length &&
							itemPacked.CoordZ + item.Dim3 <= container.Width)
                        {
							//Empacota
							item.CoordX = itemPacked.CoordX + itemPacked.Dim1;
							item.CoordY = 0;
							item.CoordZ = itemPacked.CoordZ;

							item.PackDimX = item.Dim1;
							item.PackDimY = item.Dim2;
							item.PackDimZ = item.Dim3;

							item.ItemBaseId = null;
							item.IsPacked = true;

							packedVolume = packedVolume + item.Volume;
							itemsPackedInOrder.Add(item);

							//Verifica altura
							if (container.Height < item.Dim2 * (item.ItensEmpilhados.Count + 2))
							{
								item.EmpilhamentoDisponivel = false;
							}
							else
							{
								item.EmpilhamentoDisponivel = true;
							}

							//Verifica peso maximo
							if ((item.ItensEmpilhados.Count + 1) * item.Peso > item.PesoMaximo)
							{
								item.EmpilhamentoDisponivel = false;
							}

							break;
                        }
					}
				}

				//Verifica se empacotou para a direita
				if (item.IsPacked)
					continue;

				//Tenta empacotar para frente
				foreach (var itemPacked in itemsPacked)
				{
					var itemNoGap = itemsPacked.Where(i =>
					((i.CoordX >= itemPacked.CoordX &&
					i.CoordX < itemPacked.CoordX + item.Dim1) ||
					(itemPacked.CoordX >= i.CoordX &&
					itemPacked.CoordX < i.CoordX + i.Dim1)) &&
					i.CoordZ >= itemPacked.CoordZ + itemPacked.Dim3 &&
					i.CoordZ < itemPacked.CoordZ + itemPacked.Dim3 + item.Dim3).FirstOrDefault();

					if (itemNoGap == null)
					{
						//Verificar se esta dentro dos limites do container
						if (itemPacked.CoordX + item.Dim1 <= container.Length &&
							itemPacked.CoordZ + itemPacked.Dim3 + item.Dim3 <= container.Width)
						{
							//Empacota
							item.CoordX = itemPacked.CoordX;
							item.CoordY = 0;
							item.CoordZ = itemPacked.CoordZ + itemPacked.Dim3;

							item.PackDimX = item.Dim1;
							item.PackDimY = item.Dim2;
							item.PackDimZ = item.Dim3;

							item.ItemBaseId = null;
							item.IsPacked = true;

							packedVolume = packedVolume + item.Volume;
							itemsPackedInOrder.Add(item);

							//Verifica altura
							if (container.Height < item.Dim2 * (item.ItensEmpilhados.Count + 2))
							{
								item.EmpilhamentoDisponivel = false;
							}
							else
							{
								item.EmpilhamentoDisponivel = true;
							}

							//Verifica peso maximo
							if ((item.ItensEmpilhados.Count + 1) * item.Peso > item.PesoMaximo)
							{
								item.EmpilhamentoDisponivel = false;
							}

							break;
						}
					}
				}
			}
		}

		private void PackLayersSuperiores()
        {
			var tipoDeItemIdValidos = itemsPackedInOrder.Where(i => i.EmpilhamentoDisponivel && i.CoordY == 0).Select(i => i.TipoDeItemId).Distinct();

			foreach(var item in itemsToPack.Where(i => !i.IsPacked && tipoDeItemIdValidos.Contains(i.TipoDeItemId)))
            {
				var itemBase = itemsPackedInOrder.Where(i => i.TipoDeItemId == item.TipoDeItemId && i.EmpilhamentoDisponivel && i.CoordY == 0).OrderBy(i => i.CoordZ).OrderBy(i => i.ItensEmpilhados.Count).FirstOrDefault();

				if(itemBase != null)
                {
					item.CoordX = itemBase.CoordX;
					item.CoordY = itemBase.Dim2 * (itemBase.ItensEmpilhados.Count + 1);
					item.CoordZ = itemBase.CoordZ;

					item.PackDimX = item.Dim1;
					item.PackDimY = item.Dim2;
					item.PackDimZ = item.Dim3;

					item.ItemBaseId = itemBase.ID;
					item.IsPacked = true;

					itemBase.ItensEmpilhados.Add(item);

					packedVolume = packedVolume + item.Volume;
					itemsPackedInOrder.Add(item);

					//Verifica altura
					if (container.Height < itemBase.Dim2 * (itemBase.ItensEmpilhados.Count + 2))
                    {
						itemBase.EmpilhamentoDisponivel = false;
					}

					//COMENTADO POIS O QUE IMPORTA É O PESO MAXIMO DO ITEM BASE
					//Verifica peso maximo
					//if ((item.ItensEmpilhados.Count + 1) * item.Peso > item.PesoMaximo)
					//{
					//	item.EmpilhamentoDisponivel = false;
					//}
				}
			}
        }

		#endregion Private Methods

		#region Private Classes

		/// <summary>
		/// A list that stores all the different lengths of all item dimensions.
		/// From the master's thesis:
		/// "Each Layerdim value in this array represents a different layer thickness
		/// value with which each iteration can start packing. Before starting iterations,
		/// all different lengths of all box dimensions along with evaluation values are
		/// stored in this array" (p. 3-6).
		/// </summary>
		private class Layer
		{
			/// <summary>
			/// Gets or sets the layer dimension value, representing a layer thickness.
			/// </summary>
			/// <value>
			/// The layer dimension value.
			/// </value>
			public decimal LayerDim { get; set; }

			/// <summary>
			/// Gets or sets the layer eval value, representing an evaluation weight
			/// value for the corresponding LayerDim value.
			/// </summary>
			/// <value>
			/// The layer eval value.
			/// </value>
			public decimal LayerEval { get; set; }
		}

		/// <summary>
		/// From the master's thesis:
		/// "The double linked list we use keeps the topology of the edge of the 
		/// current layer under construction. We keep the x and z coordinates of 
		/// each gap's right corner. The program looks at those gaps and tries to 
		/// fill them with boxes one at a time while trying to keep the edge of the
		/// layer even" (p. 3-7).
		/// </summary>
		[DataContract]
		private class ScrapPad
		{
			/// <summary>
			/// Gets or sets the x coordinate of the gap's right corner.
			/// </summary>
			/// <value>
			/// The x coordinate of the gap's right corner.
			/// </value>
			[DataMember]
			public decimal CumX { get; set; }

			/// <summary>
			/// Gets or sets the z coordinate of the gap's right corner.
			/// </summary>
			/// <value>
			/// The z coordinate of the gap's right corner.
			/// </value>
			[DataMember]
			public decimal CumZ { get; set; }

			/// <summary>
			/// Gets or sets the following entry.
			/// </summary>
			/// <value>
			/// The following entry.
			/// </value>
			public ScrapPad Post { get; set; }

			/// <summary>
			/// Gets or sets the previous entry.
			/// </summary>
			/// <value>
			/// The previous entry.
			/// </value>
			public ScrapPad Pre { get; set; }
		}

		#endregion Private Classes
	}
}