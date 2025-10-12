<template>
  <div class="wrapper">
    <div class="container">
      <div class="header">
        <div class="logo-circle">
          <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M22 10v6M2 10l10-5 10 5-10 5z"></path>
            <path d="M6 12v5c3 3 9 3 12 0v-5"></path>
          </svg>
        </div>
        <h1>Monte seu Currículo</h1>
        <p>Preencha seus dados profissionais e acadêmicos</p>
      </div>

      <div class="progress-bar">
        <div :class="['progress-step', { active: step >= 1 }]">
          <div class="step-circle">1</div>
          <span>Dados Pessoais</span>
        </div>
        <div class="progress-line"></div>
        <div :class="['progress-step', { active: step >= 2 }]">
          <div class="step-circle">2</div>
          <span>Experiência</span>
        </div>
        <div class="progress-line"></div>
        <div :class="['progress-step', { active: step >= 3 }]">
          <div class="step-circle">3</div>
          <span>Formação</span>
        </div>
      </div>

      <!-- ETAPA 1: DADOS PESSOAIS -->
      <div v-if="step === 1" class="step-content">
        <h2 class="step-title">Dados Pessoais</h2>
        
        <div class="form-group">
          <label>Nome Completo*</label>
          <input type="text" v-model="curriculo.nomeCompleto" required />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Data de Nascimento</label>
            <input type="date" v-model="curriculo.dataNascimento" />
          </div>
          <div class="form-group">
            <label>Telefone*</label>
            <input type="tel" v-model="curriculo.telefone" required />
          </div>
        </div>

        <div class="form-group">
          <label>Email*</label>
          <input type="email" v-model="curriculo.email" required />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Cidade</label>
            <input type="text" v-model="curriculo.cidade" />
          </div>
          <div class="form-group">
            <label>Estado</label>
            <input type="text" v-model="curriculo.estado" maxlength="2" placeholder="SP" />
          </div>
        </div>

        <div class="form-group">
          <label>Endereço</label>
          <input type="text" v-model="curriculo.endereco" />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>LinkedIn</label>
            <input type="url" v-model="curriculo.linkedin" placeholder="https://linkedin.com/in/..." />
          </div>
          <div class="form-group">
            <label>GitHub</label>
            <input type="url" v-model="curriculo.github" placeholder="https://github.com/..." />
          </div>
        </div>

        <div class="form-group">
          <label>Resumo Profissional</label>
          <textarea v-model="curriculo.resumoProfissional" placeholder="Fale um pouco sobre você e sua experiência..."></textarea>
        </div>

        <button class="btn-primary" @click="nextStep">Continuar</button>
      </div>

      <!-- ETAPA 2: EXPERIÊNCIAS -->
      <div v-if="step === 2" class="step-content">
        <h2 class="step-title">Experiências Profissionais</h2>

        <div class="divider"><span>Nova Experiência</span></div>

        <div class="form-group">
          <label>Empresa</label>
          <input type="text" v-model="novaExperiencia.empresa" placeholder="-" />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Cargo</label>
            <input type="text" v-model="novaExperiencia.cargo" placeholder="-" />
          </div>
          <div class="form-group">
            <label>Data Início</label>
            <input type="date" v-model="novaExperiencia.dataInicio" />
          </div>
        </div>

        <div class="form-group">
          <label>Data Fim</label>
          <input type="date" v-model="novaExperiencia.dataFim" :disabled="novaExperiencia.empregoAtual" />
          <div class="checkbox-wrapper">
            <input type="checkbox" id="empregoAtual" v-model="novaExperiencia.empregoAtual" />
            <label for="empregoAtual" style="margin: 0;">Trabalho aqui atualmente</label>
          </div>
        </div>

        <div class="form-group">
          <label>Descrição das Atividades</label>
          <textarea v-model="novaExperiencia.descricao" placeholder="-" rows="4"></textarea>
          
          <button type="button" @click="formatarComIA" class="btn-ia" :disabled="loadingIA || !novaExperiencia.descricao">
            <svg v-if="!loadingIA" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M12 2L2 7l10 5 10-5-10-5z"></path>
              <path d="M2 17l10 5 10-5M2 12l10 5 10-5"></path>
            </svg>
            <span v-if="!loadingIA">✨ Melhorar com IA</span>
            <span v-else class="loading-spinner-small"></span>
          </button>
          
          <small v-if="iaMessage" :class="['ia-message', iaMessageType]">{{ iaMessage }}</small>
        </div>

        <button class="btn-add" @click="adicionarExperiencia">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Adicionar Experiência
        </button>

        <div v-if="curriculo.experiencias.length > 0" class="item-list">
          <div v-for="(exp, index) in curriculo.experiencias" :key="index" class="item-card">
            <button class="btn-remove" @click="removerExperiencia(index)">Remover</button>
            <h4>{{ exp.cargo }} - {{ exp.empresa }}</h4>
            <p>{{ formatarPeriodo(exp.dataInicio, exp.dataFim, exp.empregoAtual) }}</p>
            <p v-if="exp.descricao">{{ exp.descricao }}</p>
          </div>
        </div>

        <div class="button-group">
          <button class="btn-secondary" @click="prevStep">Voltar</button>
          <button class="btn-primary" @click="nextStep">Continuar</button>
        </div>
      </div>

      <!-- ETAPA 3: FORMAÇÃO E CERTIFICAÇÕES -->
      <div v-if="step === 3" class="step-content">
        <h2 class="step-title">Formação Acadêmica</h2>

        <div class="divider"><span>Nova Formação</span></div>

        <div class="form-group">
          <label>Instituição</label>
          <input type="text" v-model="novaFormacao.instituicao" placeholder="-" />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Curso</label>
            <input type="text" v-model="novaFormacao.curso" placeholder="-" />
          </div>
          <div class="form-group">
            <label>Nível</label>
            <select v-model="novaFormacao.nivel">
              <option value="">Selecione</option>
              <option value="Técnico">Técnico</option>
              <option value="Graduação">Graduação</option>
              <option value="Pós-graduação">Pós-graduação</option>
              <option value="Mestrado">Mestrado</option>
              <option value="Doutorado">Doutorado</option>
            </select>
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Status</label>
            <select v-model="novaFormacao.status">
              <option value="">Selecione</option>
              <option value="Cursando">Cursando</option>
              <option value="Concluído">Concluído</option>
              <option value="Trancado">Trancado</option>
            </select>
          </div>
          <div class="form-group">
            <label>Data Início</label>
            <input type="date" v-model="novaFormacao.dataInicio" />
          </div>
        </div>

        <div class="form-group">
          <label>Data Conclusão</label>
          <input type="date" v-model="novaFormacao.dataConclusao" />
        </div>

        <button class="btn-add" @click="adicionarFormacao">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Adicionar Formação
        </button>

        <div v-if="curriculo.formacoes.length > 0" class="item-list">
          <div v-for="(form, index) in curriculo.formacoes" :key="index" class="item-card">
            <button class="btn-remove" @click="removerFormacao(index)">Remover</button>
            <h4>{{ form.curso }} {{ form.nivel ? `(${form.nivel})` : '' }}</h4>
            <p>{{ form.instituicao }}</p>
            <p>{{ form.status || 'Status não informado' }} {{ formatarPeriodo(form.dataInicio, form.dataConclusao, false) }}</p>
          </div>
        </div>

        <div class="divider"><span>Certificações</span></div>

        <div class="form-group">
          <label>Nome da Certificação</label>
          <input type="text" v-model="novaCertificacao.nome" placeholder="-" />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Instituição</label>
            <input type="text" v-model="novaCertificacao.instituicao" placeholder="-" />
          </div>
          <div class="form-group">
            <label>Data Conclusão</label>
            <input type="date" v-model="novaCertificacao.dataConclusao" />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Carga Horária</label>
            <input type="number" v-model="novaCertificacao.cargaHoraria" placeholder="80" />
          </div>
          <div class="form-group">
            <label>URL do Certificado</label>
            <input type="url" v-model="novaCertificacao.urlCertificado" placeholder="https://..." />
          </div>
        </div>

        <button class="btn-add" @click="adicionarCertificacao">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Adicionar Certificação
        </button>

        <div v-if="curriculo.certificacoes.length > 0" class="item-list">
          <div v-for="(cert, index) in curriculo.certificacoes" :key="index" class="item-card">
            <button class="btn-remove" @click="removerCertificacao(index)">Remover</button>
            <h4>{{ cert.nome }}</h4>
            <p>{{ cert.instituicao }}</p>
            <p v-if="cert.cargaHoraria">Carga horária: {{ cert.cargaHoraria }}h</p>
            <p v-if="cert.dataConclusao">Concluído em: {{ formatarData(cert.dataConclusao) }}</p>
            <p v-if="cert.urlCertificado"><a :href="cert.urlCertificado" target="_blank">Ver certificado</a></p>
          </div>
        </div>

        <div class="button-group">
          <button class="btn-secondary" @click="prevStep">Voltar</button>
          <button class="btn-primary" @click="salvarCurriculo" :disabled="loading">
            <span v-if="!loading">Salvar Currículo</span>
            <span v-else class="loading-spinner"></span>
          </button>
        </div>
      </div>

      <div v-if="successMessage" class="alert alert-success">
        {{ successMessage }}
      </div>

      <div v-if="errorMessage" class="alert alert-error">
        {{ errorMessage }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CurriculoForm',
  data() {
    return {
      step: 1,
      loading: false,
      loadingIA: false,
      successMessage: '',
      errorMessage: '',
      iaMessage: '',
      iaMessageType: '',
      
      curriculo: {
        usuarioId: '',
        nomeCompleto: '',
        dataNascimento: '',
        telefone: '',
        email: '',
        endereco: '',
        cidade: '',
        estado: '',
        linkedin: '',
        github: '',
        resumoProfissional: '',
        experiencias: [],
        formacoes: [],
        certificacoes: []
      },

      novaExperiencia: {
        empresa: '',
        cargo: '',
        dataInicio: '',
        dataFim: '',
        empregoAtual: false,
        descricao: ''
      },

      novaFormacao: {
        instituicao: '',
        curso: '',
        nivel: '',
        status: '',
        dataInicio: '',
        dataConclusao: ''
      },

      novaCertificacao: {
        nome: '',
        instituicao: '',
        dataConclusao: '',
        cargaHoraria: '',
        urlCertificado: ''
      }
    }
  },
  
  created() {
    const usuarioLogado = localStorage.getItem('usuario');
    
    if (usuarioLogado) {
      const usuario = JSON.parse(usuarioLogado);
      this.curriculo.usuarioId = usuario.id;
      this.curriculo.nomeCompleto = usuario.nome || '';
      this.curriculo.email = usuario.email || '';
    } else {
      console.warn('⚠️ Nenhum usuário logado encontrado');
      this.$router.push('/login');
    }
  },

  methods: {
    nextStep() {
      if (this.step < 3) {
        this.step++;
      }
    },
    
    prevStep() {
      if (this.step > 1) {
        this.step--;
      }
    },

    adicionarExperiencia() {
      if (!this.novaExperiencia.empresa && !this.novaExperiencia.cargo) {
        this.errorMessage = 'Preencha pelo menos a empresa OU o cargo da experiência!';
        setTimeout(() => this.errorMessage = '', 3000);
        return;
      }

      if (!this.novaExperiencia.empresa && this.novaExperiencia.cargo) {
        this.novaExperiencia.empresa = this.novaExperiencia.cargo;
      }
      
      if (!this.novaExperiencia.cargo && this.novaExperiencia.empresa) {
        this.novaExperiencia.cargo = 'Colaborador';
      }

      if (!this.novaExperiencia.descricao) {
        this.novaExperiencia.descricao = `Experiência profissional na área de ${this.novaExperiencia.empresa || this.novaExperiencia.cargo}`;
      }

      this.curriculo.experiencias.push({ ...this.novaExperiencia });
      
      this.novaExperiencia = {
        empresa: '',
        cargo: '',
        dataInicio: '',
        dataFim: '',
        empregoAtual: false,
        descricao: ''
      };
      
      this.successMessage = '✓ Experiência adicionada com sucesso!';
      setTimeout(() => this.successMessage = '', 2000);
    },

    removerExperiencia(index) {
      this.curriculo.experiencias.splice(index, 1);
    },

    adicionarFormacao() {
      if (!this.novaFormacao.instituicao && !this.novaFormacao.curso) {
        this.errorMessage = 'Preencha pelo menos a instituição OU o curso!';
        setTimeout(() => this.errorMessage = '', 3000);
        return;
      }

      if (!this.novaFormacao.instituicao && this.novaFormacao.curso) {
        this.novaFormacao.instituicao = 'Instituição de Ensino';
      }
      
      if (!this.novaFormacao.curso && this.novaFormacao.instituicao) {
        this.novaFormacao.curso = 'Formação Acadêmica';
      }

      this.curriculo.formacoes.push({ ...this.novaFormacao });
      
      this.novaFormacao = {
        instituicao: '',
        curso: '',
        nivel: '',
        status: '',
        dataInicio: '',
        dataConclusao: ''
      };
      
      this.successMessage = '✓ Formação adicionada com sucesso!';
      setTimeout(() => this.successMessage = '', 2000);
    },

    removerFormacao(index) {
      this.curriculo.formacoes.splice(index, 1);
    },

    adicionarCertificacao() {
      if (!this.novaCertificacao.nome && !this.novaCertificacao.instituicao) {
        this.errorMessage = 'Preencha pelo menos o nome OU a instituição!';
        setTimeout(() => this.errorMessage = '', 3000);
        return;
      }

      if (!this.novaCertificacao.nome && this.novaCertificacao.instituicao) {
        this.novaCertificacao.nome = `Certificação - ${this.novaCertificacao.instituicao}`;
      }
      
      if (!this.novaCertificacao.instituicao && this.novaCertificacao.nome) {
        this.novaCertificacao.instituicao = 'Instituição Certificadora';
      }

      this.curriculo.certificacoes.push({ ...this.novaCertificacao });
      
      this.novaCertificacao = {
        nome: '',
        instituicao: '',
        dataConclusao: '',
        cargaHoraria: '',
        urlCertificado: ''
      };
      
      this.successMessage = '✓ Certificação adicionada com sucesso!';
      setTimeout(() => this.successMessage = '', 2000);
    },

    removerCertificacao(index) {
      this.curriculo.certificacoes.splice(index, 1);
    },

    async formatarComIA() {
      if (!this.novaExperiencia.descricao || this.novaExperiencia.descricao.trim().length < 10) {
        this.iaMessage = 'ℹ️ Digite pelo menos 10 caracteres para melhorar a descrição';
        this.iaMessageType = 'info';
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);
        return;
      }

      this.loadingIA = true;
      this.iaMessage = '🤖 Melhorando sua descrição...';
      this.iaMessageType = 'info';

      try {
        const iaFormattingService = (await import('@/services/iaFormattingService')).default;
        
        const descricaoMelhorada = await iaFormattingService.formatarDescricaoProfissional(
          this.novaExperiencia.descricao,
          this.novaExperiencia.cargo,
          this.novaExperiencia.empresa
        );
        
        this.novaExperiencia.descricao = descricaoMelhorada;
        
        this.iaMessage = '✅ Descrição melhorada com sucesso!';
        this.iaMessageType = 'success';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);

      } catch (error) {
        console.error('Erro ao formatar com IA:', error);
        this.iaMessage = '❌ Erro ao melhorar. Tente novamente.';
        this.iaMessageType = 'error';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);
      } finally {
        this.loadingIA = false;
      }
    },

    async salvarCurriculo() {
      this.loading = true;
      this.errorMessage = '';
      this.successMessage = '';
      
      try {
        const payload = {
          usuarioId: this.curriculo.usuarioId,
          nomeCompleto: this.curriculo.nomeCompleto,
          dataNascimento: this.curriculo.dataNascimento || null,
          telefone: this.curriculo.telefone,
          email: this.curriculo.email,
          endereco: this.curriculo.endereco || null,
          cidade: this.curriculo.cidade || null,
          estado: this.curriculo.estado || null,
          linkedin: this.curriculo.linkedin || null,
          github: this.curriculo.github || null,
          resumoProfissional: this.curriculo.resumoProfissional || null,
          excluido: false,
          
          experiencias: this.curriculo.experiencias.map(exp => ({
            empresa: exp.empresa,
            cargo: exp.cargo,
            dataInicio: exp.dataInicio || null,
            dataFim: exp.empregoAtual ? null : (exp.dataFim || null),
            empregoAtual: exp.empregoAtual,
            descricao: exp.descricao || null
          })),
          
          formacoes: this.curriculo.formacoes.map(form => ({
            instituicao: form.instituicao,
            curso: form.curso,
            nivel: form.nivel || null,
            status: form.status || null,
            dataInicio: form.dataInicio || null,
            dataConclusao: form.dataConclusao || null
          })),
          
          certificacoes: this.curriculo.certificacoes.map(cert => ({
            nome: cert.nome,
            instituicao: cert.instituicao,
            dataConclusao: cert.dataConclusao || null,
            cargaHoraria: cert.cargaHoraria ? parseInt(cert.cargaHoraria) : null,
            urlCertificado: cert.urlCertificado || null
          }))
        };

        console.log('📋 Payload para API:', JSON.stringify(payload, null, 2));
        
        this.successMessage = 'Currículo salvo com sucesso! Redirecionando...';
        
        setTimeout(() => {
          this.$router.push('/dashboard');
        }, 2000);
        
      } catch (error) {
        console.error('Erro ao salvar currículo:', error);
        
        if (error.response) {
          this.errorMessage = error.response.data.message || 'Erro ao salvar currículo';
        } else if (error.request) {
          this.errorMessage = 'Erro de conexão. Verifique se a API está rodando.';
        } else {
          this.errorMessage = 'Erro inesperado ao salvar currículo';
        }
      } finally {
        this.loading = false;
      }
    },

    formatarPeriodo(inicio, fim, atual) {
      const dataInicio = inicio || 'N/A';
      const dataFim = atual ? 'Atual' : (fim || 'N/A');
      return `${dataInicio} - ${dataFim}`;
    },

    formatarData(data) {
      if (!data) return '';
      const [ano, mes, dia] = data.split('-');
      return `${dia}/${mes}/${ano}`;
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

.wrapper {
  min-height: 100vh;
  width: 100vw;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #ffffff 0%, #2BD2FF 52%, #2BFF88 100%);
  padding: 40px 20px;
  overflow-y: auto;
}

.container {
  background: white;
  border-radius: 24px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  padding: 48px 40px;
  width: 100%;
  max-width: 720px;
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

.header {
  text-align: center;
  margin-bottom: 40px;
}

.logo-circle {
  width: 72px;
  height: 72px;
  background: linear-gradient(135deg, #02beb5 0%, #048a8f 100%);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 24px;
  color: white;
}

.header h1 {
  font-size: 28px;
  font-weight: 700;
  color: #1a202c;
  margin-bottom: 8px;
}

.header p {
  font-size: 15px;
  color: #718096;
}

.progress-bar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 40px;
  padding: 0 20px;
}

.progress-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  flex: 1;
}

.step-circle {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: #e2e8f0;
  color: #a0aec0;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  font-size: 16px;
  transition: all 0.3s ease;
}

.progress-step.active .step-circle {
  background: linear-gradient(135deg, #02beb5 0%, #048a8f 100%);
  color: white;
}

.progress-step span {
  font-size: 12px;
  color: #a0aec0;
  font-weight: 500;
  text-align: center;
}

.progress-step.active span {
  color: #02beb5;
  font-weight: 600;
}

.progress-line {
  flex: 1;
  height: 2px;
  background: #e2e8f0;
  margin: 0 8px;
  margin-bottom: 28px;
}

.step-content {
  animation: fadeIn 0.4s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateX(20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.step-title {
  font-size: 22px;
  font-weight: 600;
  color: #1a202c;
  margin-bottom: 24px;
  padding-bottom: 12px;
  border-bottom: 2px solid #e2e8f0;
}

.form-group {
  margin-bottom: 20px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

label {
  display: block;
  font-size: 14px;
  font-weight: 600;
  color: #2d3748;
  margin-bottom: 8px;
}

input, select, textarea {
  width: 100%;
  padding: 14px 16px;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  font-size: 15px;
  transition: all 0.3s ease;
  background: #f7fafc;
  font-family: inherit;
}

textarea {
  resize: vertical;
  min-height: 100px;
}

input:focus, select:focus, textarea:focus {
  outline: none;
  border-color: #02beb5;
  background: white;
  box-shadow: 0 0 0 3px rgba(2, 190, 181, 0.1);
}

.checkbox-wrapper {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-top: 12px;
}

.checkbox-wrapper input[type="checkbox"] {
  width: 20px;
  height: 20px;
  cursor: pointer;
}

.btn-primary {
  width: 100%;
  padding: 16px;
  background: linear-gradient(135deg, #02beb5 0%, #048a8f 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 52px;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(2, 190, 181, 0.3);
}

.btn-primary:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.btn-secondary {
  padding: 16px;
  background: white;
  color: #02beb5;
  border: 2px solid #02beb5;
  border-radius: 12px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-add {
  padding: 12px 24px;
  background: linear-gradient(135deg, #02beb5 0%, #048a8f 100%);
  color: white;
  border: none;
  border-radius: 10px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  margin-top: 12px;
  transition: all 0.3s ease;
}

.btn-add:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 15px rgba(2, 190, 181, 0.3);
}

.btn-ia {
  margin-top: 12px;
  padding: 12px 20px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 8px;
  justify-content: center;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.25);
  width: 100%;
}

.btn-ia:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

.btn-ia:active:not(:disabled) {
  transform: translateY(0);
}

.btn-ia:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-ia svg {
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
}

.loading-spinner-small {
  width: 18px;
  height: 18px;
  border: 3px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

.ia-message {
  display: block;
  margin-top: 12px;
  font-size: 13px;
  font-weight: 600;
  padding: 12px 16px;
  border-radius: 10px;
  text-align: center;
  animation: fadeIn 0.3s ease;
}

.ia-message.success {
  background: #f0fff4;
  color: #2f855a;
  border: 1px solid #9ae6b4;
}

.ia-message.error {
  background: #fff5f5;
  color: #c53030;
  border: 1px solid #feb2b2;
}

.ia-message.info {
  background: #ebf8ff;
  color: #2c5282;
  border: 1px solid #90cdf4;
}

.button-group {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
  margin-top: 24px;
}

.item-list {
  margin-top: 20px;
  margin-bottom: 30px;
}

.item-card {
  background: #f7fafc;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  padding: 16px;
  margin-bottom: 12px;
  position: relative;
  animation: fadeIn 0.3s ease;
}

.item-card h4 {
  color: #1a202c;
  margin-bottom: 8px;
  font-size: 16px;
  padding-right: 80px;
}

.item-card p {
  color: #718096;
  font-size: 14px;
  margin-bottom: 4px;
}

.item-card a {
  color: #02beb5;
  text-decoration: none;
  font-weight: 600;
}

.btn-remove {
  position: absolute;
  top: 16px;
  right: 16px;
  background: #feb2b2;
  color: #c53030;
  border: none;
  border-radius: 8px;
  padding: 8px 12px;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-remove:hover {
  background: #fc8181;
}

.divider {
  display: flex;
  align-items: center;
  text-align: center;
  margin: 32px 0 24px;
  color: #a0aec0;
  font-size: 14px;
  font-weight: 600;
}

.divider::before,
.divider::after {
  content: '';
  flex: 1;
  border-bottom: 2px solid #e2e8f0;
}

.divider span {
  padding: 0 16px;
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

.alert-success {
  background: #f0fff4;
  color: #2f855a;
  border: 1px solid #9ae6b4;
}

.alert-error {
  background: #fff5f5;
  color: #c53030;
  border: 1px solid #feb2b2;
}

@media (max-width: 768px) {
  .container {
    padding: 32px 24px;
  }

  .header h1 {
    font-size: 24px;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .button-group {
    grid-template-columns: 1fr;
  }
}
</style>