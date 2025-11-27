import type { promises } from 'dns';
import type { Dish } from './DishService';
import { ApiService } from './apiService';
import type { APIResponse } from 'playwright';

export interface Menu {
    menuId: number;
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

    public async getMenusByUserId(): Promise<Menu[]> {
        const response: any = await this.ApiService.get('Menu/all');

        const menus = response?.menu || response;

        if (Array.isArray(response)) {
            const arrayMenus = menus.map((menu: any) => ({
                menuId: menu.menuId,
                menuName: menu.menuName,
                userName: menu.userName,
                theme: menu.theme,
                userId: menu.userId,
                dishes: menu.dishes || []
            }));
            return arrayMenus;
        }
        return response as Menu[];
    }

    public async createMenu(menu: Menu): Promise<Menu> {
        const response = await this.ApiService.post('menus', menu);
        return response as Menu;
    }

    public async updateMenu(menu: Menu): Promise<Menu> {
        const response = await this.ApiService.patch(`menus/${menu.menuId}`, menu);
        return response as Menu;
    }

    public async deleteMenu(menuId: number): Promise<ApiResponse> {
        const response = await this.ApiService.delete(`menus/${menuId}`);
        return response as ApiResponse;
    }
}