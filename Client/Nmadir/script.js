


function ShowInfo(){
    let name = document.getElementById("userName").value;
    let age = document.getElementById("userAge").value;

    let infoHtml = `<p>Name : ${name}</p><br><p>Age : ${age}</p>`

    document.getElementById("info").innerHTML = infoHtml;

}




const fruits = ["Apple", "Banana", "Cherry"];
let listHtml = "<ol>";

for (let fruit of fruits) {
  listHtml += `<li>${fruit}</li>`;
}

listHtml += "</ul>";

document.getElementById("content").innerHTML = listHtml;


