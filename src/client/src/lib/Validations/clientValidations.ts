

export interface IValidationResult{

    value: string;
    valid: boolean;
}

function ValidationsBuilder(pattern :RegExp | RegExp[], validate: string){
    const normValue = validate.trimStart();

    const handleArrayPattern = () =>{
        const arrPat = pattern as RegExp[];  //wrap in pattern array

        const result = { valid: true, value: normValue } as IValidationResult; //initialize object

        arrPat.forEach(x => result.valid = result.valid && x.test(normValue)) //iterate, test, assing

        return result;
    }

    //const handleSingle: () =>{}  //its here for eductational purposes, i forgor how to do typhinting definitions earlier. Im ok. its fine.

    const handleOnePattern: () => IValidationResult = () =>{
        const pat : RegExp = pattern as RegExp;

        return {
            valid: pat.test(normValue),
            value: normValue
        } as IValidationResult;
    }

    if(Array.isArray(pattern))
        return handleArrayPattern();

    return handleOnePattern();
}
//##TODO: ADD ID VALIDATIONS
export const validateField = {


    validateThemeName: (name: string) : IValidationResult=>{ 
        const normName = name.trimStart()
        //typo fix
        const menuPattern = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(?:\.webp|png|jpg|jpeg)?$/; // made group file extension group optional

        return {
            valid: menuPattern.test(normName),
            value: normName
        } as IValidationResult;
     },

    validateUserName: (username: string) : IValidationResult=>{

        const normuser = username.trimStart();
        const userPattern = /[A-z]{1}[a-z]+/; 
        const emoji = /([\p{L}\p{So}\p{Sk}\s])?/u;
        
        return { 
            valid: userPattern.test(normuser) && emoji.test(normuser),
            value: normuser

         } as IValidationResult
    },

    validateMenuName: (themeName: string) =>{
        return ValidationsBuilder(/^(?:(?:[a-zA-Z0-9]|[\u00a9\u00ae]|[\u2000-\u3300]|[\ud83c-\ud83e][\ud800-\udfff])+(?: ){0,1})*$/, themeName)
    },

    validateDishName: (dishName: string) =>{
        return ValidationsBuilder(/^[\p{L}\p{So}\p{Sk}\s]+\s?$/u, dishName);
    },

    validateId: (id :string | number) : boolean=> {
        let number : number= 0;
        number = id as number;
        if(!Number.isInteger(id))
            number = Number.parseInt(id as string);
            
        return 0 < number && number < Number.MAX_SAFE_INTEGER
    }
}