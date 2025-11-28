<script lang="ts">
    import {ApiService} from '$lib/services/apiService.ts';
    import { AccountService } from "$lib/services/accountService";
    import type { RegisterRequest, UserResponse, ErrorResponse } from "$lib/services/accountService";
	import { goto } from '$app/navigation';
    

    const apiService = new ApiService();
    const accountService = new AccountService(apiService);

    let username = "";
    let password = "";
    let email = "";
    let errorMessage = "";
    let loading = false;

    async function handleRegister(){
        if(!username || !password || !email){
            errorMessage = "All fields are required.";
            return;
        }
        loading = true;

        const registerRequest: RegisterRequest ={
            Username:username,
            Password: password,
            Email: email
        }
        const response = await accountService.registerAccount(registerRequest);
        console.log('Register response:', response);

        if('Message' in response){
            errorMessage = (response as ErrorResponse).Details;
            console.error('Register error:', errorMessage);
            loading = false;
            return;
        }
        
        if('code' in response){
            errorMessage = (response as any).message || 'An error occurred';
            console.error('API error:', response);
            loading = false;
            return;
        }

        const userResponse = response as UserResponse;
        console.log("Registerd user: " + userResponse.Username);

        await goto('/login');
    }

</script>

<div class="register-container">
    <form on:submit|preventDefault={handleRegister}>
        <h2>Register</h2>

        <div class="form-group">
            <label for="username">Username</label>
            <input type="text" bind:value={username} disabled={loading} required>
            {#if errorMessage}
                <p class="error-message">{errorMessage}</p>
            {/if}
        </div>
        <div class="form-group">
            <label for="email">Email</label>
            <input type="email" bind:value={email} disabled={loading} required>
            {#if errorMessage}
                <p class="error-message">{errorMessage}</p>
            {/if}
        </div>
        <div class="form-group">
            <label for="password">Password</label>
            <input type="password" bind:value={password} disabled={loading} required>
            {#if errorMessage}
                <p class="error-message">{errorMessage}</p>
            {/if}
        </div>
        
        <button type="submit" disabled={loading}>{loading ? 'Registering...' : 'Register'}</button>
        <a href="/login">| Already have an account? Login</a>
    </form>
</div>

<style>


input {
    width: 10%;
    padding: 4px;
    box-sizing: border-box;
    border: 1px solid #000000;
    border-radius: 4px;
}

</style>