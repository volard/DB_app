using CommunityToolkit.Mvvm.Messaging.Messages;
using DB_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Services.Messages;

public class ShowProductDetailsMessage : ValueChangedMessage<ProductWrapper>
{
    public ShowProductDetailsMessage(ProductWrapper value) : base(value)
    {
    }
}
