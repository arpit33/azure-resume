using Newtonsoft.Json;
namespace Company.@functions
 {
public class Counter
 {
    [JsonProperty(ProperyName="Id")]
public string Id {get:set:}
[JsonProperty(PropertyName = "count")]
public int Count {get:set:}
}

}