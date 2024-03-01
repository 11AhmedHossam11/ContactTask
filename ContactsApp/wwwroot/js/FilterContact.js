function applyFilter() {
    var nameInput, phoneInput, addressInput, notesInput, nameFilter, phoneFilter, addressFilter, notesFilter, table, tr, td, i, nameTxtValue, phoneTxtValue, addressTxtValue, notesTxtValue;
    nameInput = document.getElementById("nameFilter");
    phoneInput = document.getElementById("phoneFilter");
    addressInput = document.getElementById("addressFilter");
    notesInput = document.getElementById("notesFilter");
    nameFilter = nameInput.value.toUpperCase();
    phoneFilter = phoneInput.value.toUpperCase();
    addressFilter = addressInput.value.toUpperCase();
    notesFilter = notesInput.value.toUpperCase();
    table = document.getElementById("contactsTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        tdName = tr[i].getElementsByTagName("td")[0];
        tdPhone = tr[i].getElementsByTagName("td")[1];
        tdAddress = tr[i].getElementsByTagName("td")[2];
        tdNotes = tr[i].getElementsByTagName("td")[3];
        if (tdName || tdPhone || tdAddress || tdNotes) {
            nameTxtValue = tdName.textContent || tdName.innerText;
            phoneTxtValue = tdPhone.textContent || tdPhone.innerText;
            addressTxtValue = tdAddress.textContent || tdAddress.innerText;
            notesTxtValue = tdNotes.textContent || tdNotes.innerText;
            if (nameTxtValue.toUpperCase().indexOf(nameFilter) > -1 &&
                phoneTxtValue.toUpperCase().indexOf(phoneFilter) > -1 &&
                addressTxtValue.toUpperCase().indexOf(addressFilter) > -1 &&
                notesTxtValue.toUpperCase().indexOf(notesFilter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}