var requests = new Array();

function ajax_stop()
{
	for(var i=0; i<requests.length; i++)
	{
		if(requests[i] != null)
			requests[i].abort();
	}
}

function ajax_create_request(context)
{
	for(var i=0; i<requests.length; i++)
	{
		if(requests[i].readyState == 4)
		{
			requests[i].abort();
			requests[i].context = null;
			return requests[i];
		}
	}

	var pos = requests.length;
	
	requests[pos] = Object();
	requests[pos].obj = new ActiveXObject('Microsoft.XMLDOM');
	requests[pos].context = context;
	
	return requests[pos];
}

function ajax_request(url, data, callback, context)
{
	var request = ajax_create_request(context);
	var async = typeof(callback) == 'function';

	request.obj.async = async;
		
	if(async) request.obj.onreadystatechange = function()
	{
		if(request.obj.readyState == 4)
			callback(new ajax_response(request));
	}

	request.obj.load(url + '&_return=xml&' + data);

	if(!async)
	{
		return new ajax_response(request);
	}

	return;
}

function ajax_response(xmldoc)
{
	this.request = xmldoc.obj;
	this.error = null;
	this.value = null;
	this.context = xmldoc.context;

	if(xmldoc.obj != null && xmldoc.obj.documentElement != null)
	{
		this.value = object_from_json(xmldoc);
			
		if(this.value.error)
		{
			this.error = this.value.error;
			this.value = null;
		}	
	}
	
	return this;
}

function enc(s)
{
	return escape(escape(s).replace(/\+/g,'%2C').replace(/\""/g,'%22').replace(/\'/g,'%27'));
}

function object_from_json(request)
{
	if(request.obj.documentElement.tagName != "Ajax")
		return request.obj;
	
	var r = null;
	eval('r=' + request.obj.documentElement.text + ';');
	return r;
}

function ajax_error(name, description, number)
{
	this.name = name;
	this.description = description;
	this.number = number;

	return this;
}

ajax_error.prototype.toString = function()
{
	return this.name + " " + this.description;
}
