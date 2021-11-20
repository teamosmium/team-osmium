public IActionResult Summary()
{
    var claimsIdentity = (ClaimsIdentity)User.Identity;
    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

    ShoppingCartVM = new ShoppingCartVM()
    {
        OrderHeader = new Models.OrderHeader(),
        ListCart=_unitOfWork.ShoppingCart.GetAll(c=>c.ApplicationUserId==claim.Value,includeProperties:"Product")
    };
    
    ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.
                    GetFirstOrDefault(c=>c.Id==claim.Value,includeProperties:"Company");

    foreach (var list in ShoppingCartVM.ListCart)
    {
        list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
        ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
    }
    ShoppingCartVM.OrderHeader.Name= ShoppingCartVM.OrderHeader.ApplicationUser.Name;
    ShoppingCartVM.OrderHeader.PhoneNumber= ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
    ShoppingCartVM.OrderHeader.StreetAddress= ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
    ShoppingCartVM.OrderHeader.City= ShoppingCartVM.OrderHeader.ApplicationUser.City;
    ShoppingCartVM.OrderHeader.State= ShoppingCartVM.OrderHeader.ApplicationUser.State;
    ShoppingCartVM.OrderHeader.PostalCode= ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

    return View(ShoppingCartVM);
}