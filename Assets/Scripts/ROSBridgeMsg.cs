using System.Collections;
using System.Text;
using SimpleJSON;


// Declare the format that our topic node will be listening and waiting for
public class ROSBridgeMsg  {

	public ROSBridgeMsg() {}


	public virtual string ToYAMLString() {
		StringBuilder x = new StringBuilder();
		x.Append("{");
		x.Append("}");
		return x.ToString();
	}

	public static string Advertise(string messageTopic, string messageType) {
		return "{\"op\": \"advertise\", \"topic\": \"" + messageTopic + "\", \"type\": \"" + messageType + "\"}";
	}
	
	public static string UnAdvertise(string messageTopic) {
		return "{\"op\": \"unadvertise\", \"topic\": \"" + messageTopic + "\"}";
	}
	
	public static string Publish(string messageTopic, string message) {
		return "{\"op\": \"publish\", \"topic\": \"" + messageTopic + "\", \"msg\": " + message + "}";
	}
	
	public static string Subscribe(string messageTopic) {
		return "{\"op\": \"subscribe\", \"topic\": \"" + messageTopic +  "\"}";
	}
	
	public static string Subscribe(string messageTopic, string messageType) {
		return "{\"op\": \"subscribe\", \"topic\": \"" + messageTopic +  "\", \"type\": \"" + messageType + "\"}";
	}
	
	public static string UnSubscribe(string messageTopic) {
		return "{\"op\": \"unsubscribe\", \"topic\": \"" + messageTopic +  "\"}";
	}
	
	public static string CallService(string service, string args) {
		if((args == null)|| args.Equals(""))
			return "{\"op\": \"call_service\", \"service\": \"" + service +  "\"}";
		else
			return "{\"op\": \"call_service\", \"service\": \"" + service +  "\", \"args\" : " + args + "}";
	}
	
	public static string CallService(string service) {
		return "{\"op\": \"call_service\", \"service\": \"" + service +  "\"}";
	}
	
}
