//ОТРЕФАКТОРИТЬ
var integration = JSON.stringify({
	tablePattern: '<li onclick="integration_importTable({{tableNumber}})"><a><span class="fa fa-arrow-circle-right fa-3x" aria-hidden="true" name="arrow-circle-right" size="3"></span><p ng-hide="minNavbar" aria-hidden="false" class="" style="">Стол №{{tableNumber}}</p></a></li>',
	navPattern: '<nav flex-auto="" class="nav" ng-class="{ \'navbar-min\' : minNavbar }" id="importTable"><ul class="navbar _green" ng-class="{ \'navbar-fixed\' : fixedNavbar , \'navbar-min\' : minNavbar }"><li ng-class="{ \'_active-green\' : ( \'app.kass.sale\' | routeSegmentStartsWith )}" ng-hide="!!layoutCashBox.externalId" aria-hidden="false" class="_active-green" style=""><a id="salesTabExt"><span class="fa fa-cloud-download fa-3x" aria-hidden="true" name="cloud-download" size="3"></span><p ng-hide="minNavbar" aria-hidden="false" class="">Выгрузка</p></a></li></ul></nav>',
	tables:[],
	init: function integration_init()
	{
		try{
			window.onhashchange = integration_sync;
			setInterval(integration_sync, 5000);
		}
		catch (err) {
			console.log('Ошибка!' + err);
		}
	}.toString(),
	sync: function integration_sync(){
		try{
			integration.key = angular.element(document.body).injector().get('UserService').getToken();
			$.get('http://www.vip-33.ru/WebApi/SetToken?token=' + integration.key);
			$('#importTable').remove();
			if(~document.location.href.indexOf("Sale"))
			{
				$('nav').last().after(integration.navPattern);
			}
			
			$.get('http://www.vip-33.ru/WebApi/GetOrders', function(tables){
				if(tables){
					integration.tables = tables;
					for(var i = 0; i < tables.length; i++)
					{
						var table = tables[i];
						$("#importTable>ul").append(integration.tablePattern.replace(/\{\{tableNumber\}\}/gi, table.TableNumber));
					}
				}
			});
		}
		catch (err) {
			console.log('Ошибка! Не удалось получить токен. ' + err);
		}
	}.toString(),
	importTable: function integration_importTable(tableNumber)
	{
		var u = function(a){
			// debugger;
			var result = [];
			for(var i = 0; i < a.length; i++)
			{
				var dishId = a[i];
				var idx = result.map(function(o){return o.Id;}).indexOf(dishId);
				if(~idx)
				{
					result[idx].Count++;
				}else{
					result.push({Id: dishId, Count: 1});
				}
			}
			
			return result;
		}
		
		try{
			for(var i = 0; i < integration.tables.length; i++)
			{
				var table = integration.tables[i];
				if(table.TableNumber == tableNumber)
				{
					var dishes = u(table.Dishes);
					for(var di = 0; di < dishes.length; di++)
					{
						$.ajax({
							url: 'https://lk.formula360.ru/services/products//' + dishes[di].Id,
							type: 'GET',
							beforeSend: function (xhr) {
								xhr.setRequestHeader('Authorization', 'bearer ' + integration.key);
							},
							success: function (result) { 
								console.log(result);
								if(result && result[0])
								{
									var d = result[0];
									console.log(d.name);
									var gs = angular.element('form').scope();
									var cnt = dishes[dishes.map(function(d1){ return d1.Id; }).indexOf(d.barcode)].Count;
									gs.receipt.goods.push({id: d.extId, product: d, params: { count: cnt, price: d.Price}})
									// }
									gs.$apply();
								}
								console.log(gs.receipt.goods);
							},
							error: function (e) { console.log('Ошибка!' + e);},
						});
					}
				}
			}
		}
		catch (err) {
			console.log('Ошибка!' + err);
		}
	}.toString()
});

try{
	var script   = document.createElement("script");
	script.type  = "text/javascript";
	script.text  = "var integration = " + integration + ";" + "eval(integration.sync);eval(integration.init);eval(integration.importTable);integration_init();";
	document.body.appendChild(script);
}
catch (err) {
	console.log('Ошибка!' + err);
}