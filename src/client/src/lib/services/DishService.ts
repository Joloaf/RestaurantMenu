import { ApiService } from "./apiService";

export interface Dish {
    id: number | null;
    dishName: string;
    dishPicture: string;
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
    public async deleteDish(dishId: number) : Promise<Dish>{
        return await this.apiService.delete(`Dish/${dishId}`) as Dish;
    }
    public async upDateDish(dish: Dish) : Promise<Dish>{
        return await this.apiService.patch(`Dish`, dish) as Dish;
    }
        public async upDateTheme(id: number, file: string) : Promise<string>{
        return await this.apiService.patch(`Dish/${id}`, file) as string;
    }
}
