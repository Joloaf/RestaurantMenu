import { Capacitor } from '@capacitor/core';
import { PUBLIC_API_URL, PUBLIC_API_URL_ANDROID } from '$env/static/public';

/**
 * Gets the correct API URL based on environment and platform
 */
export function getApiUrl(): string {
	// In development, check if running on Android
	if (import.meta.env.DEV && Capacitor.isNativePlatform()) {
		return PUBLIC_API_URL_ANDROID || PUBLIC_API_URL;
	}
	
	return PUBLIC_API_URL;
}

/**
 * Fetch wrapper with automatic API URL handling
 */
export async function apiFetch(endpoint: string, options?: RequestInit) {
	const url = `${getApiUrl()}${endpoint}`;
	return fetch(url, options);
}
