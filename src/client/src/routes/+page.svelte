<script lang="ts">
	import { onMount } from 'svelte';
	import { apiFetch, getApiUrl } from '$lib/api';

	let weather: any[] = [];
	let loading = true;
	let error = '';

	onMount(async () => {
		try {
			const response = await apiFetch('/weatherforecast');
			if (response.ok) {
				weather = await response.json();
			} else {
				error = `Error: ${response.status}`;
			}
		} catch (e) {
			error = `Failed to connect: ${e}`;
		} finally {
			loading = false;
		}
	});
</script>

<h1>Welcome to Cafe Lek.</h1>
<p>API URL: {getApiUrl()}</p>

{#if loading}
	<p>Loading weather...</p>
{:else if error}
	<p style="color: red">{error}</p>
{:else}
	<h2>Weather Forecast</h2>
	<ul>
		{#each weather as day}
			<li>{day.date}: {day.temperatureC}°C - {day.summary}</li>
		{/each}
	</ul>
{/if}
