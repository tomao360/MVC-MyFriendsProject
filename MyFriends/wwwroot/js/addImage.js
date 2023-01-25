// פונקציה המציגה את הדיב והטופס של הוספת תמונה
// אם הוא מוסתר, מציגה אותו
// אחרת, מסתירה אותו
function addImage() {
    let divImage = document.getElementById("addImage");
    if (divImage.style.display == "none")
        divImage.style.display = "";
    else
        divImage.style.display = "none";
}