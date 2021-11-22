static IActionResult Summary(object claim)
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
[HttpPost]
[ActionName("Summary")]
[ValidateAntiForgeryToken]
public IactionResult SummaryPost()
{
    var claimsIdentity = (ClaimsIdentity)User.Identity;
    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
    ShoppingCartVM.OrderHeader.ApplicationUser=_unitOfWork.ApplicationUser
                                              .GetFirstOrDefault(c=>c.Id==claim.Value,includeProperties:"Company");

    ShoppingCartVM.ListCart=_unitOfWork.ShoppingCart.GetAll(c=>c.ApplicationUserId==claim.Value,includeProperties:"Product");

    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
    ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
    ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

    _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
    _unitOfWork.Save();

    List<OrderDetails> orderDetailsList = new List<OrderDetails>();
    foreach(var item in ShoppingCartVM.ListCart) 
    {
        OrderDetails orderDetails = new OrderDetails()
        {
            ProductId = item.ProductId,
            OrderId = ShoppingCartVM.OrderHeader.Id,
            Price = item.Price,
            Count = item.Count 
        };
        ShoppingCartVM.OrderHeader.OrderTotal += orderDetails.Count * orderDetails.Price;
        _unitOfWork.OrderDetails.Add(orderDetails);
        _unitOfWork.Save();
    }
}
