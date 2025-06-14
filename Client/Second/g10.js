function getValues() {
    debugger;
    // Get the values from inputs
    let name = document.getElementById("username").value;
    let pass = document.getElementById("password").value;

    // Show in console or use in code
    console.log("Name:", name);
    console.log("Password:", pass);
  }

  // script.js
function changeMessage() {
  document.getElementById("message").textContent = "You clicked the button, bro! 😎";
}
