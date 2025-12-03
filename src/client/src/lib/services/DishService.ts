import { ApiService } from "./apiService";

export interface Dish {
    Id: number | null;
    DishName: string;
    DishPicture: string;
}



export class DishService {
    constructor(private apiService: ApiService) {}
    
    public async getDishById(dishId: number): Promise<Dish> {
        const response = await this.apiService.get<Dish>(`Dish/${dishId}`);
        return response as Dish;
    }

    public async createDish(dish: Dish, menuId: string): Promise<Dish> {
        const response = await this.apiService.post(`Dish/${menuId}`, dish);
        return response as Dish;
    }
}
