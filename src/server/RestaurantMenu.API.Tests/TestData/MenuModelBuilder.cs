using System.Text;
using RestaurantMenu.API.Service.DTOs.Models;

namespace RestaurantMenu.API.Tests.TestData;

internal class MenuModelBuilder
{
    private enum Field { Id, UsId, MName, Theme, UsName }
    public MenuModelBuilder()
    {
        Model = new MenuModel();
    }

    private MenuModel Model { get; set; }
    

    private MenuModel ModelCreator(Field field, object value)
    {
        switch (field)
        {
            case Field.Id:
                return new MenuModel((int)value, Model.Menu_mame, Model.User_name, Model.Theme, Model.User_id); 
            case Field.UsId:
                return new MenuModel(Model.Id, Model.Menu_mame, Model.User_name, Model.Theme, (string)value);
            case Field.MName:
                return new MenuModel(Model.Id, (string)value, Model.User_name, Model.Theme, Model.User_id);
            case Field.Theme:
                return new MenuModel(Model.Id, Model.Menu_mame, Model.User_name, (string)value, Model.User_id);
            case Field.UsName:
                return new MenuModel(Model.Id, Model.Menu_mame, (string)value, Model.Theme, Model.User_id);
        }

        throw new AbandonedMutexException();
    }

    private string CreateInvalidUserName(bool spaces)
    {
        // int a = 65;
        // int b = 90;
        // int l = 97;
        // int le = 122;
        var builder = new StringBuilder();
        var len = Random.Shared.Next(0, 100);
        for (int i = 0; i < len; i++)
        {
            if (spaces && i ==0)
            {
                if(Random.Shared.Next(0, 2) == 0)
                    builder.Insert(0, " ", Random.Shared.Next(0, 10));
                else
                    builder.Append("      ");
                
            }
            
            if (Random.Shared.Next(0, 6) > 3)
            {
                builder.Append(LargeChar());
                continue;
            }

            builder.Append(SmallChar());
        }
        
        return builder.ToString();
    }
    
    private string CreateValidUserName()
    {
        // int a = 65;
        // int b = 90;
        // int l = 97;
        // int le = 122;
        var builder = new StringBuilder();
        builder.Append(LargeChar());
        var len = Random.Shared.Next(2, 100);
        for (int i = 0; i < len; i++)
            builder.Append(SmallChar());
        
        return builder.ToString();
    }
    private char LargeChar()
    {
        return (char)Random.Shared.Next(65, 91);
    }
    private char SmallChar()
    {
        return (char)Random.Shared.Next(97, 123);
    }
    
    private string CreateValidThemeName()
    {
        return Guid.NewGuid().ToString();
    }

    private string CreateInvalidThemeName()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < 36; i++)
            builder.Append((char)Random.Shared.Next(97, 122));
        return builder.ToString();
    }
    private string InvalidGuid()
    {
        return CreateInvalidThemeName();
    }
    public MenuModelBuilder WithId(bool valid)
    {
        if (valid)
        {
            this.Model = ModelCreator(Field.Id, Random.Shared.Next(0, 5000));
            return this;
        }
        this.Model = ModelCreator(Field.Id, Random.Shared.Next(int.MinValue, 0));
        return this;
    }
    public MenuModelBuilder WithUserName(bool valid)
    {
        if (valid)
        {
            this.Model = ModelCreator(Field.UsName, CreateValidUserName());
            return this;
        }
        
        this.Model = ModelCreator(Field.UsName, CreateInvalidUserName(false));
        return this;
    }

    public MenuModelBuilder WithIdentityUserId(bool valid)
    {
        if (valid)
        {
            this.Model = ModelCreator(Field.UsId, Guid.NewGuid().ToString());
            return this;
        }
        
        this.Model = ModelCreator(Field.UsId, InvalidGuid());
        return this;
    }
    public MenuModelBuilder WithName(bool valid)
    {
        if (valid)
        {
            this.Model = ModelCreator(Field.MName, CreateValidUserName());
            return this;
        }
        this.Model = ModelCreator(Field.MName, CreateInvalidUserName(true));
        return this;
    }

    public MenuModelBuilder WithThemeName(bool valid)
    {
        if (valid)
        {
            this.Model = ModelCreator(Field.Theme, CreateValidThemeName());
            return this;
        }
        this.Model = ModelCreator(Field.Theme, CreateInvalidThemeName());
        
        return this;
    }


    public MenuModel Build()
    {
        return this.Model;
    }
}