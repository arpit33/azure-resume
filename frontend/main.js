window.addEventListener('DOMContentLoaded',(event) =>{
    getVisitCount();
})

const functionApiUrl = 'http://getresumecount01.azurewebsites.net';
const localFunctionApi = 'http://localhost:7071/api/GetGetResumeCount01';

const getVisitCount = () => {
let count = 30;
fetch(functionApi).then(response => {
    return response.json()
}).then(response =>{
    console.log("website called function API");
    count = response.count;
    document.getElementById("counter").innerText = count;
}).catch(function(error){
    console.log(error);
});
return count;

}
