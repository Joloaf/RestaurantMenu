<script lang="ts">
    import { ApiService } from "$lib/services/apiService";
    import { AccountService } from "$lib/services/accountService";
    import type { LoginRequest, UserResponse, ErrorResponse } from "$lib/services/accountService";
	import { goto } from "$app/navigation";


    const apiService = new ApiService();
    const accountService = new AccountService(apiService);
    let username = "";
    let password = "";
    let errorMessage = "";
    let loading = false;

    
    
    async function handleLogin() {

        if(!username || !password){
            errorMessage = "Username and Password are required.";
            return;
        }

        loading = true;

        const loginRequest: LoginRequest = {
            Username: username,
            Password: password
        };

        const response = await accountService.loginAccount(loginRequest);
        console.log('Login response:', response);

        if('Message' in response){
            errorMessage = (response as ErrorResponse).Message;
            console.error('Login error:', errorMessage);
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
        console.log("Login in as user: " + userResponse.Username);
        console.log("Login in as user.Id: " + userResponse.Id);
        console.log(userResponse)

        try {
            await goto(`/MenuChoice/${userResponse.id}`);
        } catch (e) {
            console.error('Navigation error:', e);
        }
    }
    

</script>

<div class="login-container">
    <form on:submit|preventDefault={handleLogin}>
        <h2>Login</h2>

        <div class="form-group">
            <label for="username">Username</label>
            <input type="text" bind:value={username} disabled={loading} required>

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

        <button type="submit" disabled={loading}>{loading ? 'Logging in...' : 'Login'}</button>
        <a href="/register">Register</a>
    </form>
</div>

<style>
	




</style>