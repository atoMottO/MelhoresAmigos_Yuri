<template>
  <div class="cadastro-wrapper">
    <div class="cadastro-container">
      <div class="cadastro-header">
        <div class="logo-circle">
          <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path>
            <circle cx="9" cy="7" r="4"></circle>
            <path d="M22 21v-2a4 4 0 0 0-3-3.87"></path>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
          </svg>
        </div>
        <h1>Crie sua conta</h1>
        <p>Preencha os dados abaixo para começar</p>
      </div>

      <form @submit.prevent="handleCadastro" class="cadastro-form">
        <div class="form-group">
          <label for="nome">Nome*</label>
          <div class="input-wrapper">
            <svg class="input-icon" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
              <circle cx="12" cy="7" r="4"></circle>
            </svg>
            <input 
              type="text" 
              id="nome" 
              v-model="formUser.nome" 
              
              required
            />
          </div>
        </div>

        <div class="form-group">
          <label for="email">Email</label>
          <div class="input-wrapper">
            <svg class="input-icon" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
              <polyline points="22,6 12,13 2,6"></polyline>
            </svg>
            <input 
              type="email" 
              id="email" 
              v-model="formUser.email" 
              required 
            />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="password">Senha</label>
            <div class="input-wrapper">
              <svg class="input-icon" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
                <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
              </svg>
              <input 
                type="password" 
                id="password" 
                v-model="formUser.password" 
                placeholder="••••••••"
                required 
                minlength="6"
              />
            </div>
            <small class="input-hint">Mínimo 6 caracteres</small>
          </div>

          <div class="form-group">
            <label for="confirmPassword">Confirmar</label>
            <div class="input-wrapper">
              <svg class="input-icon" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
                <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
              </svg>
              <input 
                type="password" 
                id="confirmPassword" 
                v-model="confirmPassword" 
                placeholder="••••••••"
                required 
              />
            </div>
          </div>
        </div>

        <button type="submit" class="btn-primary" :disabled="loading">
          <span v-if="!loading">Criar Conta</span>
          <span v-else class="loading-spinner"></span>
        </button>

        <div v-if="errorMessage" class="alert alert-error">
          {{ errorMessage }}
        </div>

        <div v-if="successMessage" class="alert alert-success">
          {{ successMessage }}
        </div>
      </form>

      <div class="divider">
        <span>ou</span>
      </div>

      <div class="login-link">
        <p>Já tem uma conta? <router-link to="/login">Faça login</router-link></p>
      </div>
    </div>
  </div>
</template>

<script>
import usuarioService from '@/services/usuarioService';

export default {
  name: 'Cadastro',
  data() {
    return {
      formUser: {
        nome: '',
        email: '',
        password: '',
      },
      confirmPassword: '',
      errorMessage: '',
      successMessage: '',
      loading: false
    }
  },
  methods: {
    async handleCadastro() {
      this.errorMessage = '';
      this.successMessage = '';

      // Validações
      if (this.nome.length < 3) {
        this.errorMessage = 'Nome deve ter no mínimo 3 caracteres';
        return;
      }

      if (this.password.length < 6) {
        this.errorMessage = 'Senha deve ter no mínimo 6 caracteres';
        return;
      }

      if (this.password !== this.confirmPassword) {
        this.errorMessage = 'As senhas não conferem';
        return;
      }

      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(this.email)) {
        this.errorMessage = 'Email inválido';
        return;
      }

      this.loading = true;

      try {
        const novoUsuario = await usuarioService.adicionarUsuario({
          nome: this.nome,
          email: this.email,
          senha: this.password
        });

        this.successMessage = 'Cadastro realizado com sucesso! Redirecionando...';
        localStorage.setItem('usuario', JSON.stringify(novoUsuario));

        setTimeout(() => {
          this.$router.push('/login');
        }, 2000);

      } catch (error) {
        console.error('Erro ao cadastrar:', error);
        
        if (error.response) {
          this.errorMessage = error.response.data.message || 'Erro ao realizar cadastro';
        } else if (error.request) {
          this.errorMessage = 'Erro de conexão com o servidor. Verifique se a API está rodando.';
        } else {
          this.errorMessage = 'Erro inesperado ao realizar cadastro';
        }
      } finally {
        this.loading = false;
      }
    }
  }
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.cadastro-wrapper {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #ffffff 0%, #2BD2FF 52%, #2BFF88 100%);
  padding: 20px;
}

.cadastro-container {
  background: white;
  border-radius: 24px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  padding: 48px 40px;
  width: 100%;
  max-width: 520px;
  animation: slideUp 0.5s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.cadastro-header {
  text-align: center;
  margin-bottom: 40px;
}

.logo-circle {
  width: 72px;
  height: 72px;
  background: #02beb598;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 24px;
  color: white;
}

.cadastro-header h1 {
  font-size: 28px;
  font-weight: 700;
  color: #1a202c;
  margin-bottom: 8px;
}

.cadastro-header p {
  font-size: 15px;
  color: #718096;
}

.cadastro-form {
  margin-bottom: 24px;
}

.form-group {
  margin-bottom: 20px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.form-group label {
  display: block;
  font-size: 14px;
  font-weight: 600;
  color: #2d3748;
  margin-bottom: 8px;
}

.input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}

.input-icon {
  position: absolute;
  left: 16px;
  color: #00000046;
  pointer-events: none;
}

.input-wrapper input {
  width: 100%;
  padding: 14px 16px 14px 48px;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  font-size: 15px;
  transition: all 0.3s ease;
  background: #f7fafc;
}

.input-wrapper input:focus {
  outline: none;
  border-color: #667eea;
  background: white;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.input-wrapper input::placeholder {
  color: #cbd5e0;
}

.input-hint {
  display: block;
  margin-top: 6px;
  font-size: 12px;
  color: #a0aec0;
}

.btn-primary {
  width: 100%;
  padding: 16px;
  background: #02beb598;
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  margin-top: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 52px;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(102, 126, 234, 0.3);
}

.btn-primary:active:not(:disabled) {
  transform: translateY(0);
}

.btn-primary:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.loading-spinner {
  width: 20px;
  height: 20px;
  border: 3px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.alert {
  padding: 12px 16px;
  border-radius: 10px;
  font-size: 14px;
  margin-top: 16px;
  animation: shake 0.4s ease;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-10px); }
  75% { transform: translateX(10px); }
}

.alert-error {
  background: #fff5f5;
  color: #c53030;
  border: 1px solid #feb2b2;
}

.alert-success {
  background: #f0fff4;
  color: #2f855a;
  border: 1px solid #9ae6b4;
}

.divider {
  display: flex;
  align-items: center;
  text-align: center;
  margin: 32px 0;
  color: #a0aec0;
  font-size: 14px;
}

.divider::before,
.divider::after {
  content: '';
  flex: 1;
  border-bottom: 1px solid #e2e8f0;
}

.divider span {
  padding: 0 16px;
}

.login-link {
  text-align: center;
}

.login-link p {
  color: #718096;
  font-size: 15px;
}

.login-link a {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
  transition: color 0.3s ease;
}

.login-link a:hover {
  color: #02a6ac;
  text-decoration: underline;
}

@media (max-width: 580px) {
  .cadastro-container {
    padding: 32px 24px;
  }
  
  .cadastro-header h1 {
    font-size: 24px;
  }

  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>