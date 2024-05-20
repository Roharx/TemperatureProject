namespace api.DTOs.AccountOffice;

public class CreateAccountOfficeDto
{
    public CreateAccountOfficeDto(int account_id, int office_id, int account_rank)
    {
        Account_id = account_id;
        Office_id = office_id;
        Account_rank = account_rank;
    }
    public int Account_id { get; set; }
    public int Office_id { get; set; }
    public int Account_rank { get; set; }
}