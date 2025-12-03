import type { promises } from 'dns';
import type { Dish } from './DishService';
import { ApiService } from './apiService';
import type { APIResponse } from 'playwright';

export interface Menu {
    menuId: string | null;
    menuName: string;
    userName: string;
    theme: string;
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
            return response.map((menu: any) => ({
                menuId: menu.id,
                menuName: menu.menuName,
                userName: menu.userName,
                theme: menu.theme,
                dishes: menu.dishes || []
            }));
        }
        return [];
    }


    public async createMenu(menu: Menu): Promise<Menu> {
        const response = await this.ApiService.post('Menu/', new MenuModel(menu));
        return response as Menu;
    }

    public async updateMenu(menu: Menu): Promise<Menu> {
        const response = await this.ApiService.patch(`Menu/${menu.menuId}`, new MenuModel(menu));
        return response as Menu;
    }

    public async deleteMenu(menuId: string): Promise<ApiResponse> {
        const response = await this.ApiService.delete(`Menu/${menuId}`);
        return response as ApiResponse;
    }
}

class MenuModel{

    constructor(public menu: Menu){
        this.id = menu.menuId;
        this.user_name = menu.userName;
        this.theme = menu.theme;
        this.menu_name = menu.menuName;
    }
    public id: string | null;
    public menu_name: string;
    public user_name: string;
    public theme: string;
}
