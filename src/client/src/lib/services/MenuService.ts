import type { promises } from 'dns';
import type { Dish } from './DishService';
import { ApiService } from './apiService';
import type { APIResponse } from 'playwright';

export interface Menu {
    menuId: string | null;
    menuName: string;
    theme: string;
    userName: string;
    dishes: Dish[];
}

export interface ApiResponse{
    success: boolean;
    message: string;
    data?: unknown;
}

export class MenuService {
    constructor(private ApiService: ApiService) {}

/*     public async getMenuByMenuId(userId: number): Promise<Menu> {
        const response = await this.ApiService.get<MenuModel>(`Menu/single/${userId}`);
        
        return { 
            menuId : response.id,
            userId : response.user_id,
            menuName : response.menu_name,
            theme : response.theme,
            userName : response.userName

        } as Menu;
    } */
    public async getMenusByUserId(): Promise<Menu[]> {
        const response: any = await this.ApiService.get(`Menu/all`);

        if (Array.isArray(response)) {
            // API returns camelCase, map to our interface
            return response as Menu[];
        }
        return response;
    }


    public async createMenu(menu: Menu): Promise<Menu> {
        const response = await this.ApiService.post('Menu/', menu);
        console.log("---------------------------------------------------------")
        console.log(response)
        console.log("---------------------------------------------------------")
        return response as Menu;
    }

    public async updateMenu(menu: Menu): Promise<Menu | ApiResponse> {
        const response = await this.ApiService.patch(`Menu/${menu.menuId}`, menu);
        console.log("***************API**UPDATE**************")
        console.log(response)
        console.log("***************API**UPDATE**************")
        return response as Menu | ApiResponse;
    }

    public async deleteMenu(menuId: string): Promise<ApiResponse> {
        const response = await this.ApiService.delete(`Menu/${menuId}`);
        return response as ApiResponse;
    }
}


