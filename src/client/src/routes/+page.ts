import { MenuService } from '$lib/services/MenuService';
import { ApiService } from '$lib/services/apiService';
import { cacheHandlerActions } from '$lib/../stores/cacheHandlerService';
import type { PageLoad } from './$types';

export const load: PageLoad = async () => {
	// Initialize cache
	cacheHandlerActions.initializeCache();
	
	const apiService = new ApiService();
	const menuService = new MenuService(apiService);
	
	// Get cached data
	const activeCache = cacheHandlerActions.getActiveCache();
	
	// If cache is stale or empty, fetch fresh data
	if (cacheHandlerActions.isCacheStale() || activeCache.menus.length === 0) {
		try {
			cacheHandlerActions.setLoading(true);
			
			const menus = await menuService.getMenusByUserId();
			
			cacheHandlerActions.setMenus(menus);
			cacheHandlerActions.setLoading(false);
		} catch (error) {
			console.error('Error fetching menus:', error);
			cacheHandlerActions.setLoading(false);
		}
	}
	
	// Return data from cache
	const cache = cacheHandlerActions.getActiveCache();
	
	return {
		menus: cache.menus,
		currentMenu: cache.currentMenu,
		orders: cache.orders,
		isLoading: cache.isLoading
	};
};
