import { PUBLIC_API_URL } from "$env/static/public";

export async function getAllUsersDev() : Promise<string[]> {

    // eslint-disable-next-line prefer-const
    let response = await fetch(`${PUBLIC_API_URL}/Development/Users/GetAll`) //disabled cus i can... twas a nonissue.
    const result :string[] = [];

    if(response.ok)
       return (await response.json()) as string[];

    result[0] = await response.text()

    return result;
}