$(document).ready(function () {

    // PODACI OD INTERESA *******************************************************
    var host = 'http://' + window.location.host;
    var kosarkasiEndpoint = "/api/kosarkasi/";
    var kluboviEndpoint = "/api/klubovi/"

    var token = null;
    var headers = {};
    var formAction = "Create";
    var editingId = "";
    var idForEdit;

    // PUNJENJE TABELE PODACIMA *******************************************************
    getKosarkasi();

    // KLIK *******************************************************
    $("body").on("click", "#registracijaPrijava", divPrijava);
    $("body").on("click", "#btnRegistracija", divRegistracija);
    $("body").on("click", "#btnPocetak", divPocetak);
    $("body").on("click", "#btnAdd", addItem);
    $("body").on("click", "#btnEditItem", editItem);
    $("body").on("click", "#btnDeleteItem", deleteItem);
    $("body").on("click", "#btnQuit", quit);

// DIV registracijaPrijava *************************************************************
    function divPrijava() {
        document.getElementById("info1").classList.add("hidden");
        document.getElementById("divPrijava").classList.remove("hidden");
    }

// CLICK btnPOCETAK *************************************************************
    function divPocetak() {
        document.getElementById("info1").classList.remove("hidden");
        document.getElementById("divPrijava").classList.add("hidden");
        document.getElementById("prijavljen2").innerHTML = "";
    }

    // REGISTRACIJA KORISNIKA *******************************************************
    $("#formRegistracija").submit(function (e) {
        e.preventDefault();

        var email = $("#regEmail").val();   // test@gmail.com
        var loz1 = $("#regLoz").val();      // Test@123
        var loz2 = $("#regLoz2").val();     // Test@123

        // objekat koji se salje
        var sendData = {
            "Email": email,
            "Password": loz1,
            "ConfirmPassword": loz2
        };

        $.ajax({
            type: "POST",
            url: host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            //$("#prijavljen").append("Uspešna registracija. Možete se prijaviti na sistem.");
            document.getElementById("divRegistracija").classList.add("hidden");
            document.getElementById("divPrijava").classList.remove("hidden");

        }).fail(function (data) {
            alert("Greska prilikom registracije."/*+ data.responseText*/);
        });
        document.getElementById("regEmail").value = "";
        document.getElementById("regLoz").value = "";
        document.getElementById("regLoz2").value = "";
    });

    // PRIJAVA KORISNIKA *******************************************************
    $("#formPrijava").submit(function (e) {
        e.preventDefault();
        document.getElementById("prijavljen2").innerHTML = "";

        var email = $("#priEmail").val();
        var loz = $("#priLoz").val();

        // objekat koji se salje
        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            "type": "POST",
            "url": host + "/Token",
            "data": sendData

        }).done(function (data) {
            token = data.access_token;
            document.getElementById("priEmail").value = "";
            document.getElementById("priLoz").value = "";
            document.getElementById("divPrijava").classList.add("hidden");
            document.getElementById("info").classList.remove("hidden");
            $("#prijavljen").empty().append(/*"Prijavljen korisnik: " +*/ data.userName);
            document.getElementById("pretraga").classList.remove("hidden");
            //document.getElementById("dodavanje").classList.remove("hidden");

            let elements = document.getElementsByClassName("showHide");
            for (var i = 0; i < elements.length; i++) {
                elements[i].classList.remove("hidden");
            }

            populateSelectList();
            getKosarkasi();

        }).fail(function (data) {
            //document.getElementById("priEmail").value = "";
            //document.getElementById("priLoz").value = "";
            alert("Greska prilikom prijave.");
        });
    });

    // ODJAVA *******************************************************
    $("#odjava").click(function () {
        token = null;
        headers = {};
        formAction = "Create";
        editingId = "";

        $("#prijavljen").empty();
        document.getElementById("info").classList.add("hidden");
        document.getElementById("pretraga").classList.add("hidden");
        document.getElementById("dodavanje").classList.add("hidden");
        document.getElementById("divPrijava").classList.remove("hidden");

        let elements = document.getElementsByClassName("showHide");
        for (var i = 0; i < elements.length; i++) {
            elements[i].classList.add("hidden");
        }
    })

    // PUNJENJE TABELE PODACIMA *******************************************************
    function populateTable(data) {
        $("#tableData1").empty();

        var body = document.getElementById('table1').getElementsByTagName('tbody')[0];
        for (var i = 0; i < data.length; i++) {
            var row = body.insertRow(i);
            row.classList.add("text-center");

            var cell0 = row.insertCell(0);
            var cell1 = row.insertCell(1);
            var cell2 = row.insertCell(2);
            var cell3 = row.insertCell(3);
            var cell4 = row.insertCell(4);
            var cell5 = row.insertCell(5);
            var cell6 = row.insertCell(6);

            cell0.innerHTML = data[i].Naziv;
            cell1.innerHTML = data[i].Godina;
            cell2.innerHTML = data[i].klub.Naziv;
            cell3.innerHTML = data[i].BrojUtakmica;
            cell4.innerHTML = data[i].ProsecanBrojPoena;

            cell5.classList.add("showHide");
            cell6.classList.add("showHide");
            if (!token) {
                cell5.classList.add("hidden");
                cell6.classList.add("hidden");
            }

            var btn1 = document.createElement('button');
            var btn2 = document.createElement('button');
            btn1.classList.add("btn", "btn-default");
            btn2.classList.add("btn", "btn-default");
            btn1.textContent = "Obrisi";
            btn2.textContent = "Izmeni";
            btn1.value = data[i].Id;
            btn2.value = data[i].Id;
            btn1.name = "id";
            btn2.name = "id";
            btn1.id = "btnDeleteItem";
            btn2.id = "btnEditItem";
            cell5.appendChild(btn1);
            cell6.appendChild(btn2);
        }
    }

    // AJAX GET ITEMS *************************************************************
    function getKosarkasi() {
        $.ajax({
            type: "GET",
            url: host + kosarkasiEndpoint
        })
            .done(function (data, status) {
                populateTable(data);
            })
            .fail(function (data, status) {
                alert("Doslo je do greske prilikom dobavljanja stavki.\n\n" + data.responseText);
            });
    }

    // CLICK SUBMIT PRETRAGA *******************************************************
    $("#formPretraga").submit(function (e) {
        e.preventDefault();

        if (token) {
            let data = getPretraga();
            document.getElementById("najmanje").value = "";
            document.getElementById("najvise").value = "";
        }
        else {
            alert("Morate biti ulogovani.")
        }
    });

    // PRETRAGA *************************************************************
    function getPretraga() {
        if (token) {
            headers.Authorization = 'Bearer ' + token;

            // objekat koji se salje
            let sendData = {
                "Najmanje": document.getElementById("najmanje").value,
                "Najvise": document.getElementById("najvise").value
            };

            $.ajax({
                type: "POST",
                url: host + "/api/pretraga",
                data: sendData,
                "headers": headers
            })
                .done(function (data, status) {
                    populateTable(data);
                })
                .fail(function (data, status) {
                    if (data.statusText = "Not Found") {
                        alert("Ni jedna stavka ne ispunjava kriterijum pretrage.\nPokusajte pretragu sa drugim kriterijumima.");
                    }
                    else {
                        alert("Greska prilikom pretrage!");
                    }
                });
        }
        else {
            alert("Morate biti ulogovani.");
        }
    }

    // PUNJENJE SELECT LISTE *******************************************************
    function populateSelectList() {
        var sel = document.getElementById("sel1");
        //if (sel) {
        //    sel.options[sel.options.length] = new Option(name, i);
        //}
        //else {
        //    alert("Greska prilikom punjenja select liste.");
        //}
        $.ajax({
            url: host + kluboviEndpoint,
            type: "GET",
        })
            .done(function (data, status) {
                if ($('#sel1 option').length == 0) {
                    for (var i = 0; i < data.length; i++) {
                        //var option = document.createElement('option');
                        //option.value = data[i].Id;
                        //if (data[i].Id == 1) {
                        //    option.selected = true;
                        //}
                        //option.text = data[i].Naziv;
                        //sel.add(option, 0);
                        if (sel) {
                            sel.options[i] = new Option(data[i].Naziv, data[i].Id);
                        }
                    }
                }
            })
            .fail(function (data, status) {
                alert("Desila se greska prilikom popunjavanja padajuceg menija.\n\n" + data.responseText);
            });
    }

    // KLIK btnRegistracija *******************************************************
    function divRegistracija() {
        document.getElementById("prijavljen2").innerHTML = "";
        var email = $("#priEmail").val();   // test@gmail.com
        var loz = $("#priLoz").val();      // Test@123

        // objekat koji se salje
        var sendData = {
            "Email": email,
            "Password": loz,
            "ConfirmPassword": loz
        };

        $.ajax({
            type: "POST",
            url: host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            document.getElementById("priEmail").value = "";
            document.getElementById("priLoz").value = "";
            //$("#prijavljen2").append("Uspešna registracija na sistem.");
            document.getElementById("prijavljen2").innerHTML = "Uspesna registracija na sistem!";

        }).fail(function (data) {
            alert("Greška prilikom registracije!");
        });
    }

    // BRISANJE STAVKE ************************************************************
    function deleteItem() {
        var deleteId = this.value;

        if (token) {
            headers.Authorization = 'Bearer ' + token;
            $.ajax({
                url: host + kosarkasiEndpoint + deleteId,
                type: "DELETE",
                "headers": headers
            })
                .done(function (data, status) {
                    //populateTable(data);
                    getKosarkasi();
                })
                .fail(function (data, status) {
                    if (data.statusText = "Method Not Allowed") {
                        alert("Morate biti ulogovani da bi ste obrisali stavku." /*+ data.responseText*/)
                    }
                    else {
                        alert("Desila se greska prilikom brisanja stavke." /*+ data.responseText*/);
                    }
                });
        }
        else {
            alert("Morate biti ulogovani da bi ste obrisali stavku." /*+ data.responseText*/)
        }
    };

    // DODAVANJE STAVKE ************************************************************
    function addItem() {
        if (token) {
            $("#addEditForm").submit(function (e) {
                e.preventDefault();
                headers.Authorization = 'Bearer ' + token;

                var itemName = document.getElementById("itemName").value;
                var itemDate = document.getElementById("itemDate").value;
                var itemSelected = document.getElementById("sel1").value;
                var itemPrice = document.getElementById("itemPrice").value;
                var itemProsek = document.getElementById("itemProsek").value;

                // objekat koji se salje
                sendData = {
                    "Naziv": itemName,
                    "Godina": itemDate,
                    "BrojUtakmica": itemPrice,
                    "ProsecanBrojPoena": itemProsek,
                    "KlubId": itemSelected
                };

                formAction = "PUT";

                if (formAction === "Create") {
                    url = host + kosarkasiEndpoint;
                    httpMethod = "POST";
                }
                else {
                    httpMethod = "PUT";
                    url = host + kosarkasiEndpoint + idForEdit; /*editingId;*/
                    sendData.Id = idForEdit;      /*editingId;*/
                }

                $.ajax({
                    url: url,
                    type: httpMethod,
                    data: sendData,
                    "headers": headers
                })
                    .done(function (data, status) {
                        document.getElementById("itemName").value = "";
                        document.getElementById("itemPrice").value = "";
                        document.getElementById("itemDate").value = "";
                        document.getElementById("sel1").value = 1;
                        document.getElementById("dodavanje").classList.add("hidden");
                        //populateTable(data);
                        getKosarkasi();
                    })
                    .fail(function (data, status) {
                        if (formAction === "Create") {
                            var jsonResponse = JSON.parse(data.responseText);
                            var obj1 = jsonResponse.ModelState;
                            var text = "";
                            for (var key in obj1) {
                                text += obj1[key] + ".\n";
                            }
                            alert("Desila se greska prilikom dodavanja nove stavke.\n\n" + text);
                        }
                        else {
                            alert("Desila se greska prilikom editovanja stavke.\n\n" + data.responseText);
                        }
                    })
                //editingId = "";
            });
            editingId = "";
        } else {
            alert("You must be loged in to add item.")
        }
    }

    // KLIK btnQuit *******************************************************
    function quit() {
        document.getElementById("itemName").value = "";
        document.getElementById("itemDate").value = "";
        document.getElementById("itemPrice").value = "";
        document.getElementById("sel1").value = 1;
        document.getElementById("itemProsek").value = "";
        document.getElementById("dodavanje").classList.add("hidden");
    }

    // EDITOVANJE STAVKE ************************************************************
    function editItem() {
        var editId = this.value;
        if (/*token*/ 1 == 1) {
            //headers.Authorization = 'Bearer ' + token;

            // saljemo zahtev da dobavimo stavku
            $.ajax({
                url: host + kosarkasiEndpoint + editId,
                type: "GET"/*,*/
                //"headers": headers
            })
                .done(function (data, status) {
                    document.getElementById("dodavanje").classList.remove("hidden");
                    document.getElementById("itemName").value = data.Naziv;
                    document.getElementById("itemDate").value = data.Godina;
                    document.getElementById("sel1").value = data.KlubId;
                    document.getElementById("itemPrice").value = data.BrojUtakmica;
                    document.getElementById("itemProsek").value = data.ProsecanBrojPoena;
                    editingId = data.Id;
                    idForEdit = data.Id;
                    formAction = "Update";
                })
                .fail(function (data, status) {
                    document.getElementById("dodavanje").classList.add("hidden");
                    formAction = "Create";
                    alert("Greska prilikom izmene.");
                });
        } else {
            alert("Morate biti ulogovani.")
        }
    }
});