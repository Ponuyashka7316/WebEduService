function FilterSubGrid() {
    //ContactsForOrg - это собственно название сабгрида
    var subgrid = document.getElementById("ContactsForOrg");
    if (subgrid == null || subgrid.length == 0) {
        setTimeout(FilterSubGrid, 2000);
        return;
    }
    
    var requestId = Xrm.Page.data.entity.getId();
    if (!requestId) {
        return;
    }
    debugger;
    //здесь берется значение лукапа на моей форме (по заданию нужно выбрать контакты связанные с аккаунтом выбранном в данном лукапе)
    var account = Xrm.Page.getAttribute("new_lookupfield").getValue();
    //это просто дефолтное значение (на случай если account = null). Хотя по хорошему запрос вообще не нужно выполнять в случае отсутствия фирмы. Но мне лень уже менять
    var accountId = "GrishevichCorp";
    if (account) {
        //Берем id связанной фирмы(аккаунта)
        accountId = account[0].id;
    }
    //fetchxml был составлен через advanced find. Однако он сгенерировал так же и не нужные аттрибуты, пришлось их удалить
    var fetchXml =
        "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
        "  <entity name='contact'>" +
        "    <attribute name='fullname' />" +
        "    <attribute name='telephone1' />" +
        "    <attribute name='contactid' />" +
        "    <order attribute='fullname' descending='false' />" +
        "    <filter type='and'>" +
        "      <condition attribute='parentcustomerid' operator='eq' uitype='account' value='" + accountId + "' />" +
        "    </filter>" +
        "  </entity>" +
        "</fetch>";

    subgrid.control.SetParameter("fetchxml", fetchXml);
    subgrid.control.Refresh();
    //ну и в конце осталось только повесить выполнение данной функции на onload формы
}