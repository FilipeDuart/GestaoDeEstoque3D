var scene;
var camera;
var renderer;
var controls;
var itemMaterial;
var itemMaterialSelecionado;
var view3D;
var ContainerPackingResult;
var ContainersResponse;
var BalcaoAncoragem;
var LastItemRenderedIndex = -1;
var ContainerAtual;
var itemSelecionadoId = null;

$(document).ready(() => {
	view3D = new View3D();

	View3D.InitializeDrawing();

	PackContainers();
});

async function PackContainers(request) {
	return $.ajax({
		url: '/Inicio/ContainerPacking',
		type: 'POST',
		data: request,
		contentType: 'application/json; charset=utf-8',
		success: function (response) {
			ContainerPackingResult = response.PackResult;
			ContainersResponse = response.Containers;
			BalcaoAncoragem = response.BalcaoAncoragem;
			view3D.ShowPackingView(ContainerPackingResult[0]);
		}
	});
};

class View3D {
	ContainerOriginOffset = {
		x: 0,
		y: 0,
		z: 0
	}

	static InitializeDrawing() {
		var container = $('#view-3d');

		scene = new THREE.Scene();
		camera = new THREE.PerspectiveCamera(50, window.innerWidth / window.innerHeight, 0.1, 1000);
		camera.lookAt(scene.position);

		//var axisHelper = new THREE.AxisHelper( 5 );
		//scene.add( axisHelper );

		// LIGHT
		var light = new THREE.PointLight(0xffffff);
		light.position.set(0, 150, 100);
		scene.add(light);

		// Get the item stuff ready.
		itemMaterial = new THREE.MeshNormalMaterial({ transparent: true, opacity: 0.6 });
		itemMaterialSelecionado = new THREE.MeshNormalMaterial({ transparent: true, opacity: 1 });

		renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true }); // WebGLRenderer CanvasRenderer
		//renderer.setClearColor(0xf0f0f0);
		//renderer.setClearColor(0xffffff);
		renderer.setPixelRatio(window.devicePixelRatio);
		renderer.setSize(400, 400);
		container.append(renderer.domElement);

		controls = new THREE.OrbitControls(camera, renderer.domElement);
		window.addEventListener('resize', View3D.onWindowResize, false);

		View3D.animate();
	}

	static onWindowResize() {
		camera.aspect = window.innerWidth / window.innerHeight;
		camera.updateProjectionMatrix();
		renderer.setSize(window.innerWidth / 2, window.innerHeight / 2);
	}

	static animate() {
		requestAnimationFrame(View3D.animate);
		controls.update();
		View3D.render();
	}
	static render() {
		renderer.render(scene, camera);
	}

	ShowPackingView(containerPackingResult, ItemSelecionadoId = null) {
		itemSelecionadoId = ItemSelecionadoId;
		var containerId = containerPackingResult.ContainerID
		var container = ContainersResponse.find(elem => elem.ID == containerId);

		ContainerAtual = containerPackingResult;

		var selectedObject = scene.getObjectByName('container');
		scene.remove(selectedObject);

		for (var i = 0; i < 1000; i++) {
			var selectedObject = scene.getObjectByName('cube' + i);
			scene.remove(selectedObject);
		}

		camera.position.set(container.Length, container.Length, container.Length);

		LastItemRenderedIndex = -1;

		this.ContainerOriginOffset.x = -1 * container.Length / 2;
		this.ContainerOriginOffset.y = -1 * container.Height / 2;
		this.ContainerOriginOffset.z = -1 * container.Width / 2;

		var geometry = new THREE.BoxGeometry(container.Length, container.Height, container.Width);
		var geo = new THREE.EdgesGeometry(geometry); // or WireframeGeometry( geometry )
		var mat = new THREE.LineBasicMaterial({ color: 0x000000, linewidth: 2 });
		var wireframe = new THREE.LineSegments(geo, mat);
		wireframe.position.set(0, 0, 0);
		wireframe.name = 'container';
		scene.add(wireframe);
	}

	PackAllItemsInRender() {
		while (LastItemRenderedIndex < ContainerAtual.PackedItems.length - 1) {
			this.PackItemInRender();
        }
    }

	PackItemInRender () {
		if (LastItemRenderedIndex < ContainerAtual.PackedItems.length - 1) {
			LastItemRenderedIndex++;

			var PackedItems = ContainerAtual.PackedItems;

			var itemOriginOffset = {
				x: PackedItems[LastItemRenderedIndex].PackDimX / 2,
				y: PackedItems[LastItemRenderedIndex].PackDimY / 2,
				z: PackedItems[LastItemRenderedIndex].PackDimZ / 2
			};

			var itemGeometry = new THREE.BoxGeometry(PackedItems[LastItemRenderedIndex].PackDimX, PackedItems[LastItemRenderedIndex].PackDimY, PackedItems[LastItemRenderedIndex].PackDimZ);
			var cube;
			if (itemSelecionadoId != null, PackedItems[LastItemRenderedIndex].ItemId == itemSelecionadoId) {
				cube = new THREE.Mesh(itemGeometry, itemMaterialSelecionado);
			} else {
				cube = new THREE.Mesh(itemGeometry, itemMaterial);
            }
			cube.position.set(this.ContainerOriginOffset.x + itemOriginOffset.x + PackedItems[LastItemRenderedIndex].CoordX, this.ContainerOriginOffset.y + itemOriginOffset.y + PackedItems[LastItemRenderedIndex].CoordY, this.ContainerOriginOffset.z + itemOriginOffset.z + PackedItems[LastItemRenderedIndex].CoordZ);
			cube.name = 'cube' + LastItemRenderedIndex;
			scene.add(cube);
		}
	}

	UnpackAllItemsInRender() {
		while (LastItemRenderedIndex > -1) {
			this.UnpackItemInRender();
		}
	}

	UnpackItemInRender () {
		if (LastItemRenderedIndex > -1) {
			var selectedObject = scene.getObjectByName('cube' + LastItemRenderedIndex);
			scene.remove(selectedObject);
			LastItemRenderedIndex--;
		}
	}
}