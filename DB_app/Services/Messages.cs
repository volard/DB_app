using CommunityToolkit.Mvvm.Messaging.Messages;
using DB_app.Entities;
using DB_app.ViewModels;

namespace DB_app.Services.Messages;

// TODO it should replace all repeates below
public class AddRecordMessage<T> : ValueChangedMessage<T>
{
    public AddRecordMessage(T value) : base(value)
    {
    }
}

public class ShowRecordDetailsMessage<T> : ValueChangedMessage<T>
{
    public ShowRecordDetailsMessage(T value) : base(value)
    {
    }
}
// ----------------------------------

public class Messages : ValueChangedMessage<AddressWrapper>
{
    public Messages(AddressWrapper value) : base(value)
    {
    }
}

public class ShowAddressDetailsMessage : ValueChangedMessage<AddressWrapper>
{
    public ShowAddressDetailsMessage(AddressWrapper value) : base(value)
    {
    }
}

public class AddMedicineMessage : ValueChangedMessage<MedicineWrapper>
{
    public AddMedicineMessage(MedicineWrapper value) : base(value)
    {
    }
}

public class ShowMedicineDetailsMessage : ValueChangedMessage<MedicineWrapper>
{
    public ShowMedicineDetailsMessage(MedicineWrapper value) : base(value)
    {
    }
}

public class AddHospitalMessage : ValueChangedMessage<HospitalWrapper>
{
    public AddHospitalMessage(HospitalWrapper value) : base(value)
    {
    }
}

public class ShowHospitalDetailsMessage : ValueChangedMessage<HospitalWrapper>
{
    public ShowHospitalDetailsMessage(HospitalWrapper value) : base(value)
    {
    }
}

public class AddOrderMessage : ValueChangedMessage<OrderWrapper>
{
    public AddOrderMessage(OrderWrapper value) : base(value)
    {
    }
}

public class ShowOrderDetailsMessage : ValueChangedMessage<OrderWrapper>
{
    public ShowOrderDetailsMessage(OrderWrapper value) : base(value)
    {
    }
}


public class AddPharmacyMessage : ValueChangedMessage<PharmacyWrapper>
{
    public AddPharmacyMessage(PharmacyWrapper value) : base(value)
    {
    }
}

public class ShowPharmacyDetailsMessage : ValueChangedMessage<PharmacyWrapper>
{
    public ShowPharmacyDetailsMessage(PharmacyWrapper value) : base(value)
    {
    }
}

public class AddProductMessage : ValueChangedMessage<ProductWrapper>
{
    public AddProductMessage(ProductWrapper value) : base(value)
    {
    }
}

public class ShowProductDetailsMessage : ValueChangedMessage<ProductWrapper>
{
    public ShowProductDetailsMessage(ProductWrapper value) : base(value)
    {
    }
}

public class HospitalsChangedMessage : ValueChangedMessage<List<Hospital>>
{
    public HospitalsChangedMessage(List<Hospital> value) : base(value)
    {
    }
}
