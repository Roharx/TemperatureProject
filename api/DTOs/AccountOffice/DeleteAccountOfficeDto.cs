namespace api.DTOs.AccountOffice;

public class DeleteAccountOfficeDto
{
    public DeleteAccountOfficeDto(int account_id, int office_id)
    {
        Account_id = account_id;
        Office_id = office_id;
    }

    public int Account_id { get; set; }
    public int Office_id { get; set; }
}