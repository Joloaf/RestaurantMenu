    import { dev } from "$app/environment";
    import type { Actions } from "@sveltejs/kit";
	import { writeFileSync } from "fs";
    import { ApiService } from "$lib/services/apiService";
    import { MenuService } from "$lib/services/MenuService";
    import { DishService } from "$lib/services/DishService";
    import { cacheHandlerActions } from "../stores/cacheHandlerService";
    import type { UpdateThemeResponse } from "$lib/services/MenuService";


export const actions: Actions = {
    default: async ({ request }) => {
        const data = await request.formData();
        const files = data.getAll("picture") as File[];
        
        let path = "uploads/";
        if (dev) {
            path = "C:/Users/Marcu/Repo/RestaurantMenu/dev_picture/";
        }

        const rawId = data.get("id");
        let id: number = rawId ? parseInt(rawId as string) : 0;

        if(id === 0) {
            return { success: false, message: "Invalid ID" };
        }
        
        const type = data.get("type");
        const apiService = new ApiService();
        
        for (const file of files) {
            const fileName = crypto.randomUUID();
            console.log("Saving file: " + fileName);
            await writeFileSync(path + fileName, Buffer.from(await file.arrayBuffer()));

            // Here you can save the fileName to your database and cache if needed
                console.log(type);

             if (type === "menu") {
                console.log("Updating menu theme");
/*                 const menuService = new MenuService(apiService);
                id = parseInt(rawId as string);
                
                const oldFile = await menuService.updateTheme(id, fileName); */
            }
            if (type === "dish") {
                console.log("Updating dish theme");
/* 
                const dishService = new DishService(apiService);
                id = parseInt(rawId as string);
                const updatedDish = await dishService.upDateTheme(id, fileName); */
            } 
        }
        return { success: true };
    }
} satisfies Actions;