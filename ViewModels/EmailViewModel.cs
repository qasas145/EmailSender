public class EmailViewModel {
    public string ToEmail{get;set;}
    public string Subject{get;set;}
    public string Body{get;set;}
    public IList<FormFile> Attachments{get;set;}
    
}