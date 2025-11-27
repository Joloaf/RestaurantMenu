using System.Reflection.Metadata.Ecma335;
using System.Text;
using RestaurantMenu.API.Service.DTOs.Models;

namespace RestaurantMenu.API.Tests.TestData;

[Flags]
public enum Spaces {Start =1, Ending =2, Internal =4 }
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
            case Field.UsId:
                return new MenuModel(Model.Id, Model.Menu_name, Model.User_name, Model.Theme, (string)value);
            case Field.Theme:
                return new MenuModel(Model.Id, Model.Menu_name, Model.User_name, (string)value, Model.User_id);
            case Field.UsName:
                return new MenuModel(Model.Id, Model.Menu_name, (string)value, Model.Theme, Model.User_id);
            case Field.MName:
                return new MenuModel(Model.Id, (string)value, Model.User_name, Model.Theme, Model.User_id);
            case Field.Id:
                return new MenuModel((int)value, Model.Menu_name, Model.User_name, Model.Theme, Model.User_id); 
        }

        throw new AbandonedMutexException();
    }
    private string CreateInvalidMenuName(Spaces spaces)
    {
        //this is still valid....
        //like... the only thing making a menu name invalid is... spaces :)
        // and an Umbrella emoji. 
        var builder = new StringBuilder();
        var len = Random.Shared.Next(0, 100);
        for (int i = 0; i < len; i++)
        {
            if (Random.Shared.Next(0, 100) > 50)
            {
                builder.Append(SmallChar());
                continue;
            }
            builder.Append(LargeChar());
        }
        
        if(builder.Length == 0)
            return builder.ToString();
        
        var space = spaces;
        if (space.HasFlag(Spaces.Start))
        {
            builder.Insert(0, " ", Random.Shared.Next(2, 10));
            space &= ~Spaces.Start;
        }

        if (space.HasFlag(Spaces.Ending))
        {
            builder.Append(" ");
            space &= ~Spaces.Ending;
        }

        if (space.HasFlag(Spaces.Internal))
        {
            //i fucking hate you rider
            builder.Insert((int)(builder.Length-1/2), " ",  Random.Shared.Next(2, 10));
            space &= ~Spaces.Internal;
        }
        return builder.ToString();
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
    public MenuModelBuilder WithName(bool valid, Spaces? spaces = null)
    {
        if (valid)
        {
            this.Model = ModelCreator(Field.MName, CreateValidUserName());
            return this;
        }
        this.Model = ModelCreator(Field.MName, CreateInvalidMenuName(spaces ?? Spaces.Internal));
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