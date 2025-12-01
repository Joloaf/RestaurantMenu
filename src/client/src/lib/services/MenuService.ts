import type { promises } from 'dns';
import type { Dish } from './DishService';
import { ApiService } from './apiService';
import type { APIResponse } from 'playwright';

export interface Menu {
    menuId: number | null;
    menuName: string;
    userName: string;
    theme: string;
    userId: string;  // will we send this from backend?
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
    public async getMenusByUserId(uid :string): Promise<Menu[]> {
        const response: any = await this.ApiService.get<MenuModel>(`Menu/all/${uid}`);

        const menus = response?.menu || response;

        if (Array.isArray(response)) {
            const arrayMenus = menus.map((menu: any) => ({
                menuId: menu.id,
                menuName: menu.menu_name,
                userName: menu.user_name,
                theme: menu.theme,
                userId: menu.user_id,
                dishes: menu.dishes || []
            }));
            return arrayMenus;
        }
        return response as Menu[];
    }


    public async createMenu(menu: Menu): Promise<Menu> {
        const response = await this.ApiService.post('Menu/', new MenuModel(menu));
        return response as Menu;
    }

    public async updateMenu(menu: Menu): Promise<Menu> {
        const response = await this.ApiService.patch(`Menu/${menu.menuId}`, new MenuModel(menu));
        return response as Menu;
    }

    public async deleteMenu(menuId: number, userId: string): Promise<ApiResponse> {
        const response = await this.ApiService.delete(`Menu/${menuId}?userId=${userId}`);
        return response as ApiResponse;
    }
}

class MenuModel{

    constructor(public menu: Menu){
        this.id = menu.menuId;
        this.user_id = menu.userId;
        this.user_name = menu.userName;
        this.theme = menu.theme;
        this.menu_name = menu.menuName;
    }
    public id: number | null;
    public menu_name: string;
    public user_name: string;
    public theme: string;
    public user_id: string;
}