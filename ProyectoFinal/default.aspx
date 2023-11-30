<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProyectoFinal._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="mt-4 container card px-5 py-4">

        <h2 class="m-0 p-0">Administrador de Pagos</h2>
        <hr />
        <div class="col-lg-9 mt-3">

            <asp:Label ID="Label1" runat="server" Text="Titular de la tarjeta" CssClass="font-weight-bold mx-1"></asp:Label>
            <div class="row mb-4">
                <div class="col-md-6">
                    <asp:TextBox runat="server" ID="tbxNombre" CssClass="form-control" placeholder="Nombre(s)" > </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNombre"  ValidationGroup="btnEnviar" CssClass="mx-1" runat="server" ControlToValidate="tbxNombre" ErrorMessage="El nombre es obligatorio." ForeColor="Red" Display="Dynamic"/>
                    <asp:RegularExpressionValidator runat="server"  ValidationGroup="btnEnviar" ControlToValidate="tbxNombre" ErrorMessage="No se permiten caracteres especiales ni numeros." ValidationExpression="^[a-zA-Z]+$" Display="Dynamic" ForeColor="Red"/>
                </div>
                <div class="col-md-6">
                    <asp:TextBox runat="server" ID="tbxApellido" CssClass="form-control" placeholder="Apellido(s)"> </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvApellido" ValidationGroup="btnEnviar" CssClass="mx-1" runat="server" ControlToValidate="tbxApellido" ErrorMessage="El apellido es obligatorio." ForeColor="Red" Display="Dynamic"/>
                    <asp:RegularExpressionValidator runat="server"  ValidationGroup="btnEnviar" ControlToValidate="tbxApellido" ErrorMessage="No se permiten caracteres especiales ni numeros." ValidationExpression="^[a-zA-Z]+$" Display="Dynamic" ForeColor="Red"/>
                </div>
            </div>

            <asp:Label ID="Label2" runat="server" Text="Número de tarjeta" CssClass="font-weight-bold mx-1"></asp:Label>
            <div class="row mb-4">
                <div class="col-md-12">
                    <asp:TextBox runat="server" ID="tbxCardNbr" CssClass="form-control" placeholder="1234 5678 9123 4567" MaxLength="19" OnTextChanged="tbxCardNbr_TextChanged" AutoPostBack="true"> </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCardNbr" ValidationGroup="btnEnviar" CssClass="mx-1" runat="server" ControlToValidate="tbxCardNbr" ErrorMessage="El número de tarjeta es obligatorio." ForeColor="Red" Display="Dynamic"/>
                </div>
            </div>

            <div class="row mb-4">
                <div class="col-md-6">
                    <asp:Label ID="Label3" runat="server" Text="Fecha de caducidad" CssClass="font-weight-bold mx-1"></asp:Label>
                    <asp:TextBox runat="server" ID="tbxFC" CssClass="form-control" placeholder="MM/AA" MaxLength="5"> </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFC" ValidationGroup="btnEnviar" CssClass="mx-1" runat="server" ControlToValidate="tbxFC" ErrorMessage="La fecha de caducidad es obligatoria." ForeColor="Red" Display="Dynamic"/>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="Label4" runat="server" Text="Código de seguridad" CssClass="font-weight-bold mx-1"></asp:Label>
                    <asp:TextBox runat="server" ID="tbxCVC" CssClass="form-control" placeholder="CVC" MaxLength="3" OnTextChanged="tbxCVC_TextChanged" AutoPostBack="true"> </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCVC" ValidationGroup="btnEnviar" CssClass="mx-1" runat="server" ControlToValidate="tbxCVC" ErrorMessage="El código de seguridad es obligatorio." ForeColor="Red" Display="Dynamic"/>
                </div>
            </div>

        </div>
        <div class="d-flex justify-content-end">
            <asp:Button ID="btnEnviar" runat="server" CssClass="btn btn-primary pull-right" Text="Agregar" OnClick="btnEnviar_Click" ValidationGroup="btnEnviar"/>
        </div>
    </div>
</asp:Content>
