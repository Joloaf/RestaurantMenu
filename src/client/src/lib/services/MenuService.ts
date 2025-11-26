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

    public async getUserMenusByUserId(userId: string): Promise<Menu[]> {
        const response = await this.ApiService.get(`Menu/all/${userId}`);
        return response as Menu[];
    }

    public async getMenuByMenuId(userId: number): Promise<Menu> {
        const response = await this.ApiService.get(`Menu/single/${userId}`);
        return response as Menu;
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