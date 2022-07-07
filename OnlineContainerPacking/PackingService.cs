using OnlineContainerPacking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineContainerPacking
{
	/// <summary>
	/// The container packing service.
	/// </summary>
	public static class PackingService
	{
		/// <summary>
		/// Attempts to pack the specified containers with the specified items using the specified algorithms.
		/// </summary>
		/// <param name="containers">The list of containers to pack.</param>
		/// <param name="itemsToPack">The items to pack.</param>
		/// <param name="algorithmTypeIDs">The list of algorithm type IDs to use for packing.</param>
		/// <returns>A container packing result with lists of the packed and unpacked items.</returns>
		public static List<ContainerPackingResult> Pack(List<Container> containers, List<Item> itemsToPack, List<int> algorithmTypeIDs)
		{
			Object sync = new Object { };
			List<ContainerPackingResult> result = new List<ContainerPackingResult>();

			Parallel.ForEach(containers, container =>
			{
				ContainerPackingResult containerPackingResult = new ContainerPackingResult();
				containerPackingResult.ContainerID = container.ID;

				Parallel.ForEach(algorithmTypeIDs, algorithmTypeID =>
				{
					var jsonDefinition = new
					{
						Height = new Decimal(),
						Length = new Decimal(),
						Volume = new Decimal(),
						Width = new Decimal(),
						Items = new List<Item>()
					};

					var onlineContainerJsonSource = "{\"Height\":9.0,\"Length\":15.0,\"Volume\":1755.0,\"Width\":13.0,\"Items\":[{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":1,\"IsPacked\":true,\"Dim1\":3.0,\"Dim2\":3.0,\"Dim3\":3.0,\"CoordX\":0.0,\"CoordY\":0.0,\"CoordZ\":0.0,\"PackDimX\":3.0,\"PackDimY\":3.0,\"PackDimZ\":3.0,\"Volume\":27.000},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":1,\"IsPacked\":true,\"Dim1\":3.0,\"Dim2\":3.0,\"Dim3\":3.0,\"CoordX\":3.0,\"CoordY\":0.0,\"CoordZ\":0.0,\"PackDimX\":3.0,\"PackDimY\":3.0,\"PackDimZ\":3.0,\"Volume\":27.000},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":1,\"IsPacked\":true,\"Dim1\":3.0,\"Dim2\":3.0,\"Dim3\":3.0,\"CoordX\":6.0,\"CoordY\":0.0,\"CoordZ\":0.0,\"PackDimX\":3.0,\"PackDimY\":3.0,\"PackDimZ\":3.0,\"Volume\":27.000},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":2,\"IsPacked\":true,\"Dim1\":2.0,\"Dim2\":2.0,\"Dim3\":2.0,\"CoordX\":9.0,\"CoordY\":0.0,\"CoordZ\":0.0,\"PackDimX\":2.0,\"PackDimY\":2.0,\"PackDimZ\":2.0,\"Volume\":8.0},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":2,\"IsPacked\":true,\"Dim1\":2.0,\"Dim2\":2.0,\"Dim3\":2.0,\"CoordX\":11.0,\"CoordY\":0.0,\"CoordZ\":0.0,\"PackDimX\":2.0,\"PackDimY\":2.0,\"PackDimZ\":2.0,\"Volume\":8.0},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":2,\"IsPacked\":true,\"Dim1\":2.0,\"Dim2\":2.0,\"Dim3\":2.0,\"CoordX\":13.0,\"CoordY\":0.0,\"CoordZ\":0.0,\"PackDimX\":2.0,\"PackDimY\":2.0,\"PackDimZ\":2.0,\"Volume\":8.0},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":2,\"IsPacked\":true,\"Dim1\":2.0,\"Dim2\":2.0,\"Dim3\":2.0,\"CoordX\":9.0,\"CoordY\":0.0,\"CoordZ\":2.0,\"PackDimX\":2.0,\"PackDimY\":2.0,\"PackDimZ\":2.0,\"Volume\":8.0},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":2,\"IsPacked\":true,\"Dim1\":2.0,\"Dim2\":2.0,\"Dim3\":2.0,\"CoordX\":11.0,\"CoordY\":0.0,\"CoordZ\":2.0,\"PackDimX\":2.0,\"PackDimY\":2.0,\"PackDimZ\":2.0,\"Volume\":8.0},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":2,\"IsPacked\":true,\"Dim1\":2.0,\"Dim2\":2.0,\"Dim3\":2.0,\"CoordX\":13.0,\"CoordY\":0.0,\"CoordZ\":2.0,\"PackDimX\":2.0,\"PackDimY\":2.0,\"PackDimZ\":2.0,\"Volume\":8.0},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":2,\"IsPacked\":true,\"Dim1\":2.0,\"Dim2\":2.0,\"Dim3\":2.0,\"CoordX\":7.0,\"CoordY\":0.0,\"CoordZ\":3.0,\"PackDimX\":2.0,\"PackDimY\":2.0,\"PackDimZ\":2.0,\"Volume\":8.0},{\"ID\":0,\"PesoMaximo\":0,\"Peso\":0,\"EmpilhamentoDisponivel\":true,\"ItensEmpilhados\":[],\"TipoDeItemId\":2,\"IsPacked\":true,\"Dim1\":2.0,\"Dim2\":2.0,\"Dim3\":2.0,\"CoordX\":5.0,\"CoordY\":0.0,\"CoordZ\":3.0,\"PackDimX\":2.0,\"PackDimY\":2.0,\"PackDimZ\":2.0,\"Volume\":8.0}]}";

					var onlineContainerSource = JsonConvert.DeserializeAnonymousType(onlineContainerJsonSource, jsonDefinition);

					// Until I rewrite the algorithm with no side effects, we need to clone the item list
					// so the parallel updates don't interfere with each other.
					List<Item> items = new List<Item>();

					onlineContainerSource.Items.ForEach(item =>
					{
						item.Quantity = 1;
						//item.IsPacked = false;
						items.Add(item);

						item.ItensEmpilhados.ForEach(itemEmpilhado => items.Add(itemEmpilhado));
					});

					itemsToPack.ForEach(item =>
					{
						for (int i = 0; i < item.Quantity; i++)
							items.Add(new Item(item.ID, item.Dim1, item.Dim2, item.Dim3, 1, item.TipoDeItemId));
					});

					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					//AlgorithmPackingResult algorithmResult = algorithm.Run(container, items);
					AlgorithmPackingResult algorithmResult = new OnlineContainerPacking().OnlineRun(container, items);
					stopwatch.Stop();

					algorithmResult.PackTimeInMilliseconds = stopwatch.ElapsedMilliseconds;

					decimal containerVolume = container.Length * container.Width * container.Height;
					decimal itemVolumePacked = algorithmResult.PackedItems.Sum(i => i.Volume);
					decimal itemVolumeUnpacked = algorithmResult.UnpackedItems.Sum(i => i.Volume);

					algorithmResult.PercentContainerVolumePacked = Math.Round(itemVolumePacked / containerVolume * 100, 2);
					algorithmResult.PercentItemVolumePacked = Math.Round(itemVolumePacked / (itemVolumePacked + itemVolumeUnpacked) * 100, 2);

					var onlineContainer = new
					{
						Height = container.Height,
						Length = container.Length,
						Volume = container.Volume,
						Width = container.Width,
						Items = algorithmResult.PackedItems
					};

					var onlineContainerJson = JsonConvert.SerializeObject(onlineContainer);

					lock (sync)
					{
						containerPackingResult.AlgorithmPackingResults.Add(algorithmResult);
					}
				});

				containerPackingResult.AlgorithmPackingResults = containerPackingResult.AlgorithmPackingResults.OrderBy(r => r.AlgorithmName).ToList();

				lock (sync)
				{
					result.Add(containerPackingResult);
				}
			});

			return result;
		}

		public static void OnlinePack(List<Container> containers, List<Item> itemsToPack)
		{
			//TODO
			//*Determinar a ordem como os itens sao associados (Decidir melhor container, utilizando os TipoItemId)

			foreach(var container in containers)
            {
				var _itemsToPack = itemsToPack.Where(ip => ip.IsPacked == false).ToList();

				if (_itemsToPack.Count == 0)
					break;

				AlgorithmPackingResult algorithmResult = new OnlineContainerPacking().OnlineRun(container, _itemsToPack);

				algorithmResult.PackedItems.ForEach(pi =>
				{
					var item = itemsToPack.Find(i => i.ID == pi.ID);

					if(item != null)
                    {
						item.IsPacked = pi.IsPacked;
						item.ContainerId = container.ID;

						item.CoordX = pi.CoordX;
						item.CoordY = pi.CoordY;
						item.CoordZ = pi.CoordZ;

						item.PackDimX = pi.PackDimX;
						item.PackDimY = pi.PackDimY;
						item.PackDimZ = pi.PackDimZ;

						item.EmpilhamentoDisponivel = pi.EmpilhamentoDisponivel;
						item.ItensEmpilhados = pi.ItensEmpilhados;
						item.ItemBaseId = pi.ItemBaseId;

						container.Alterado = true;

					}
				});
			}
		}
	}
}
