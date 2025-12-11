<script lang="ts">
    import { ApiService } from "$lib/services/apiService";
    import { AccountService } from "$lib/services/accountService";
    import type { LoginRequest, UserResponse, ErrorResponse } from "$lib/services/accountService";
    import { clearCache } from "../../stores/cacheHandlerService";

    let { isOpen = $bindable(false), onSwitchToRegister = () => {} } = $props();

    const apiService = new ApiService();
    const accountService = new AccountService(apiService);
    
    let username = $state("");
    let password = $state("");
    let errorMessage = $state("");
    let loading = $state(false);

    async function handleLogin() {
        if(!username || !password){
            errorMessage = "Username and Password are required.";
            return;
        }

        loading = true;
        errorMessage = "";

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
        console.log("Logged in as user: " + userResponse.Username);
        console.log("Logged in as user.Id: " + userResponse.Id);
        console.log(userResponse);

        localStorage.setItem('user', JSON.stringify(userResponse));
        
        username = "";
        password = "";
        loading = false;
        isOpen = false;
        clearCache();
        document.location.reload();
    }

    function closeModal() {
        isOpen = false;
        errorMessage = "";
        username = "";
        password = "";
    }

    function handleSwitchToRegister() {
        closeModal();
        onSwitchToRegister();
    }

    function handleBackdropClick(e: MouseEvent) {
        if (e.target === e.currentTarget) {
            closeModal();
        }
    }
</script>

{#if isOpen}
    <div class="modal-backdrop" onclick={handleBackdropClick}>
        <div class="modal-content">
            <button class="close-btn" onclick={closeModal}>✕</button>
            
            <form onsubmit={(e) => { e.preventDefault(); handleLogin(); }}>
                <h2>Inloggning</h2>

                <div class="form-group">
                    <label for="username">Användarnamn</label>
                    <input 
                        type="text" 
                        id="username"
                        bind:value={username} 
                        disabled={loading} 
                        required
                    />
                </div>

                <div class="form-group">
                    <label for="password">Lösenord</label>
                    <input 
                        type="password" 
                        id="password"
                        bind:value={password} 
                        disabled={loading} 
                        required
                    />
                </div>

                {#if errorMessage}
                    <p class="error-message">{errorMessage}</p>
                {/if}

                <button type="submit" class="submit-btn" disabled={loading}>
                    {loading ? 'Inloggning pågår...' : 'Logga in'}
                </button>
                
                <button type="button" class="register-link" onclick={handleSwitchToRegister}>
                    Behöver du ett konto? Registrera dig!
                </button>
            </form>
        </div>
    </div>
{/if}

<style>
    .modal-backdrop {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
        backdrop-filter: blur(4px);
    }

    .modal-content {
        background: white;
        padding: 2rem;
        border-radius: 16px;
        box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
        max-width: 400px;
        width: 90%;
        position: relative;
        animation: slideIn 0.3s ease-out;
    }

    @keyframes slideIn {
        from {
            transform: translateY(-20px);
            opacity: 0;
        }
        to {
            transform: translateY(0);
            opacity: 1;
        }
    }

    .close-btn {
        position: absolute;
        top: 1rem;
        right: 1rem;
        background: transparent;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        color: #666;
        padding: 0;
        width: 2rem;
        height: 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 50%;
        transition: all 0.2s ease;
    }

    .close-btn:hover {
        background: #f0f0f0;
        color: #333;
    }

    h2 {
        margin-top: 0;
        margin-bottom: 1.5rem;
        color: #FF6B6B;
        text-align: center;
        font-size: 1.8rem;
    }

    .form-group {
        margin-bottom: 1.25rem;
    }

    label {
        display: block;
        margin-bottom: 0.5rem;
        font-weight: 600;
        color: #333;
    }

    input {
        width: 100%;
        padding: 0.75rem;
        border: 2px solid #ddd;
        border-radius: 8px;
        font-size: 1rem;
        transition: border-color 0.2s ease;
        box-sizing: border-box;
    }

    input:focus {
        outline: none;
        border-color: #FF6B6B;
    }

    input:disabled {
        background: #f5f5f5;
        cursor: not-allowed;
    }

    .error-message {
        color: #d32f2f;
        font-size: 0.875rem;
        margin-top: 0.5rem;
        margin-bottom: 0;
    }

    .submit-btn {
        width: 100%;
        padding: 0.875rem;
        background: linear-gradient(135deg, #FF6B6B 0%, #FFB347 100%);
        color: white;
        border: 2px solid #D94A4A;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: bold;
        cursor: pointer;
        transition: all 0.2s ease;
        margin-top: 1rem;
        text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.2),
                     -1px -1px 2px rgba(0, 0, 0, 0.2),
                     1px -1px 2px rgba(0, 0, 0, 0.2),
                     -1px 1px 2px rgba(0, 0, 0, 0.2);
    }

    .submit-btn:hover:not(:disabled) {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(255, 107, 107, 0.3);
    }

    .submit-btn:disabled {
        opacity: 0.6;
        cursor: not-allowed;
    }

    .register-link {
        display: block;
        width: 100%;
        text-align: center;
        margin-top: 1rem;
        color: #4ECDC4;
        background: transparent;
        border: none;
        cursor: pointer;
        font-size: 0.9rem;
        padding: 0.5rem;
    }

    .register-link:hover {
        text-decoration: underline;
    }
</style>