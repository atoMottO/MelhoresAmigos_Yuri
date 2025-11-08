<template>
  <div class="wrapper">
    <div class="container">
      <button @click="abrirModal" class="btn-back">
        Sair
      </button>
      <div class="header">
        <div class="logo-circle">
          <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M22 10v6M2 10l10-5 10 5-10 5z"></path>
            <path d="M6 12v5c3 3 9 3 12 0v-5"></path>
          </svg>
        </div>
        <h1>{{ modoEdicao ? 'Editar Currículo' : 'Criar Currículo' }}</h1>
        <p>Preencha seus dados profissionais</p>
      </div>

      <div class="progress-bar">
        <div :class="['progress-step', { active: step >= 1 }]">
          <div class="step-circle">1</div>
          <span>Pessoais</span>
        </div>
        <div class="progress-line" :class="{ active: step >= 2 }"></div>
        <div :class="['progress-step', { active: step >= 2 }]">
          <div class="step-circle">2</div>
          <span>Experiência</span>
        </div>
        <div class="progress-line" :class="{ active: step >= 3 }"></div>
        <div :class="['progress-step', { active: step >= 3 }]">
          <div class="step-circle">3</div>
          <span>Formação</span>
        </div>
        <div class="progress-line" :class="{ active: step >= 4 }"></div>
        <div :class="['progress-step', { active: step >= 4 }]">
          <div class="step-circle">4</div>
          <span>Certificações</span>
        </div>
      </div>

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
            <label>Telefone</label>
            <input 
              type="tel" 
              v-model="telefoneFormatado" 
              @input="formatarTelefone"
              placeholder="(XX) XXXXX-XXXX"
              maxlength="15"
            />
          </div>
        </div>

        <div class="form-group">
          <label>Email</label>
          <input type="email" v-model="curriculo.email" disabled/>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Estado</label>
            <EstadoDropdown v-model="curriculo.estado" />
          </div>
          <div class="form-group">
            <label>Cidade</label>
            <input type="text" v-model="curriculo.cidade" />
          </div>
        </div>

        <div class="form-group">
          <label>Endereço</label>
          <input type="text" v-model="curriculo.endereco" />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>LinkedIn</label>
            <input type="url" v-model="curriculo.linkedIn" placeholder="linkedin.com/in/..." />
          </div>
          <div class="form-group">
            <label>GitHub</label>
            <input type="url" v-model="curriculo.gitHub" placeholder="github.com/..." />
          </div>
        </div>

        <button class="btn-primary" @click="nextStepPerfil">Continuar</button>
      </div>

      <div v-if="step === 2" class="step-content">
        <h2 class="step-title">Experiências Profissionais</h2>
        <div class="form-card">
          <div class="form-group">
            <label>Empresa</label>
            <input type="text" v-model="novaExperiencia.empresa" />
          </div>

          <div class="form-row">
            <div class="form-group">
              <label>Cargo</label>
              <input type="text" v-model="novaExperiencia.cargo" />
            </div>
            <div class="form-group">
              <label>Data Início*</label>
              <input type="date" v-model="novaExperiencia.dataInicio" required/>
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
            <label>Descrição das Atividades*</label>
            <textarea v-model="novaExperiencia.descricao" rows="4" required></textarea>
            
            <button type="button" @click="formatarComIA" class="btn-ia" :disabled="loadingIA || !novaExperiencia.descricao">
              <svg v-if="!loadingIA" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M12 2L2 7l10 5 10-5-10-5z"></path>
                <path d="M2 17l10 5 10-5M2 12l10 5 10-5"></path>
              </svg>
              <span v-if="!loadingIA">Melhorar com IA</span>
              <span v-else class="loading-spinner-small"></span>
            </button>
            
            <small v-if="iaMessage" :class="['ia-message', iaMessageType]">{{ iaMessage }}</small>
          </div>

          <button class="btn-add" @click="adicionarExperiencia">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="12" y1="5" x2="12" y2="19"></line>
              <line x1="5" y1="12" x2="19" y2="12"></line>
            </svg>
            {{ editandoIndexExperiencia !== null ? 'Atualizar Experiência' : 'Adicionar Experiência' }}
          </button>
        </div>

        <div v-if="curriculo.experiencias.length > 0" class="item-list">
          <div v-for="(exp, index) in curriculo.experiencias" :key="index" class="item-card">
            <button class="btn-edit" @click="editarExperiencia(index)" title="Editar">
                <i class="fas fa-pencil"></i>
              </button>
            <button class="btn-remove" @click="removerExperiencia(index)">×</button>
            <h4>{{ exp.cargo ? exp.cargo : "Não Informado"}}</h4>
            <p class="subtitle">{{ exp.empresa ? exp.empresa : "Não Informado"}}</p>
            <p class="date">{{ formatarPeriodo(exp.dataInicio, exp.dataFim, exp.empregoAtual) }}</p>
            <p v-if="exp.descricao" class="description">{{ exp.descricao }}</p>
          </div>
        </div>

        <div class="button-group">
          <button class="btn-secondary" @click="prevStep">Voltar</button>
          <button class="btn-primary" @click="nextStep">Continuar</button>
        </div>
      </div>

      <div v-if="step === 3" class="step-content">
        <h2 class="step-title">Formação Acadêmica</h2>

        <div class="form-card">
          <div class="form-group">
            <label>Instituição</label>
            <input type="text" v-model="novaFormacao.instituicao" />
          </div>

          <div class="form-row">
            <div class="form-group">
              <label>Curso</label>
              <input type="text" v-model="novaFormacao.curso" />
            </div>
            <div class="form-group">
              <label>Nível</label>
              <NivelDropdown v-model="novaFormacao.nivel" />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label>Status</label>
              <StatusDropdown v-model="novaFormacao.status" />
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
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="12" y1="5" x2="12" y2="19"></line>
              <line x1="5" y1="12" x2="19" y2="12"></line>
            </svg>
            {{ editandoIndexFormacao !== null ? 'Atualizar Formação' : 'Adicionar Formação' }}
          </button>
        </div>

        <div v-if="curriculo.formacoes.length > 0" class="item-list">
          <div v-for="(form, index) in curriculo.formacoes" :key="index" class="item-card">
            <button class="btn-edit" @click="editarFormacao(index)" title="Editar">
              <i class="fas fa-pencil"></i>
            </button>
            <button class="btn-remove" @click="removerFormacao(index)">×</button>
            <h4>{{ form.curso }}</h4>
            <p class="subtitle">{{ form.instituicao }}</p>
            <p class="date">{{ form.nivel }} • {{ form.status }}</p>
          </div>
        </div>

        <div class="button-group">
          <button class="btn-secondary" @click="prevStep">Voltar</button>
          <button class="btn-primary" @click="nextStep">Continuar</button>
        </div>
      </div>

      <div v-if="step === 4" class="step-content">
        <h2 class="step-title">Certificações</h2>

        <div class="form-card">
          <div class="form-group">
            <label>Nome da Certificação</label>
            <input type="text" v-model="novaCertificacao.nome" />
          </div>

          <div class="form-row">
            <div class="form-group">
              <label>Instituição</label>
              <input type="text" v-model="novaCertificacao.instituicao" />
            </div>
            <div class="form-group">
              <label>Data Conclusão</label>
              <input type="date" v-model="novaCertificacao.dataConclusao" />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label>Carga Horária</label>
              <input type="number" v-model="novaCertificacao.cargaHoraria"/>
            </div>
            <div class="form-group">
              <label>URL do Certificado</label>
              <input type="url" v-model="novaCertificacao.urlCertificado" />
            </div>
          </div>

          <button class="btn-add" @click="adicionarCertificacao">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="12" y1="5" x2="12" y2="19"></line>
              <line x1="5" y1="12" x2="19" y2="12"></line>
            </svg>
            {{ editandoIndexCertificacao !== null ? 'Atualizar Certificação' : 'Adicionar Certificação' }}
          </button>
        </div>

        <div v-if="curriculo.certificacoes.length > 0" class="item-list">
          <div v-for="(cert, index) in curriculo.certificacoes" :key="index" class="item-card">
            <button class="btn-edit" @click="editarCertificacao(index)" title="Editar">
              <i class="fas fa-pencil"></i>
            </button>
            <button class="btn-remove" @click="removerCertificacao(index)">×</button>
            <h4>{{ cert.nome }}</h4>
            <p class="subtitle">{{ cert.instituicao }}</p>
            <p v-if="cert.cargaHoraria" class="date">{{ cert.cargaHoraria }}h</p>
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

    <ModalExclusao
      :show="showConfirmModal"
      :title="modalConfig.title"
      :message="modalConfig.message"
      :type="modalConfig.type"
      :confirmText="modalConfig.confirmText"
      @confirmar="confirmarRemocao"
      @fechar="showConfirmModal = false"
    />
    <ModalEncerramentoSessao
      :isOpen="modalAberto"
      @confirmar="confirmarSair"
      @fechar="fecharModal"/>
  </div>
</template>

<script>
import EstadoDropdown from '@/components/EstadoDropdown.vue'
import ModalExclusao from '@/components/ModalExclusao.vue'
import certificacaoService from '@/services/certificacaoService';
import curriculoService from '@/services/curriculoService';
import experienciaService from '@/services/experienciaService';
import formacaoService from '@/services/formacaoService';
import usuarioService from '@/services/usuarioService';
import '@fortawesome/fontawesome-free/css/all.css';
import { ref, watch } from 'vue';
import StatusDropdown from '@/components/StatusDropdown.vue';
import NivelDropdown from '@/components/NivelDropdown.vue';
import ModalEncerramentoSessao from '@/components/ModalEncerramentoSessao.vue';

export default {
  name: 'CurriculoForm',
  
  components: {
    EstadoDropdown,
    StatusDropdown,
    NivelDropdown,
    ModalExclusao,
    ModalEncerramentoSessao
  },
  data() {
    return {
      step: 1,
      editandoIndexExperiencia: null,
      editandoExperiencia: false,
      editandoIndexFormacao: null,
      editandoFormacao: null,
      editandoIndexCertificacao: null,
      editandoCertificacao: null,
      loading: false,
      loadingIA: false,
      successMessage: '',
      errorMessage: '',
      iaMessage: '',
      iaMessageType: '',
      telefoneFormatado: '',
      modoEdicao: false,
      modalAberto: false,
      curriculoId: null,
      showConfirmModal: false,
      itemParaRemover: null,
      curriculo: {},
      curriculoOriginal: {},
      modalConfig: {
        title: '',
        message: '',
        type: 'aviso',
        confirmText: 'Confirmar',
      },
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
  watch: {
    'novaExperiencia.empregoAtual': function(newValue) {
      if (newValue) {
          this.novaExperiencia.dataFim = '';
      }
    }
  },
  async created() {
    const usuarioLogado = localStorage.getItem('usuario');
    
    if (usuarioLogado) {
      console.log(usuarioLogado)
      const usuario = JSON.parse(usuarioLogado);
      this.curriculo.usuarioId = usuario.id;
      this.curriculo.email = usuario.email || '';
      this.curriculo.nomeCompleto = usuario.nome;
      if (this.$route.params.id) {
        this.modoEdicao = true;
        this.curriculoId = this.$route.params.id;
        const curriculo = await curriculoService.listarCurriculoPorId(this.curriculoId);
        
        if (curriculo) {
          this.curriculo = {
            ...curriculo,
            experiencias: [],
            formacoes: [],
            certificacoes: []
          };
          
          this.telefoneFormatado = this.formatarTelefoneString(curriculo.telefone || '');
          
          try {
            const experiencias = await experienciaService.listarExperienciaPorIdCurriculo(this.curriculoId);
            const formacoes = await formacaoService.listarFormacoesPorCurriculoId(this.curriculoId);
            const certificados = await certificacaoService.listarCertificacoesPorCurriculoId(this.curriculoId);
            
            this.curriculo.experiencias = Array.isArray(experiencias) ? experiencias : [];
            this.curriculo.formacoes = Array.isArray(formacoes) ? formacoes : [];
            this.curriculo.certificacoes = Array.isArray(certificados) ? certificados : [];
          } catch (error) {
            console.error("Erro ao carregar listas de currículo:", error);
            return this.mostrarErro('Erro ao carregar experiências, formações ou certificados.');
          }
        } else {
          return this.mostrarErro('Currículo para edição não encontrado.');
        }
      }
      this.curriculo.dataNascimento = this.curriculo.dataNascimento.split('T')[0]
      this.curriculoOriginal = JSON.parse(JSON.stringify(this.curriculo));
    } else {
      this.$router.push('/login');
    }
  },

  methods: {
    mostrarErro(mensagem){
      this.errorMessage = mensagem
      this.$nextTick(() => {
          window.scrollTo({
          top: document.body.scrollHeight,
          behavior: 'smooth'
        });
      });
      setTimeout(() => this.errorMessage = '', 3000);
      return;
    },
    resetarFormExperiencia() {
      this.novaExperiencia = {
        empresa: '',
        cargo: '',
        dataInicio: '',
        dataFim: '',
        empregoAtual: false,
        descricao: ''
      };
      this.editandoIndexExperiencia = null;
    },

    resetarFormFormacao() {
      this.novaFormacao = {
        instituicao: '',
        curso: '',
        nivel: '',
        status: '',
        dataInicio: '',
        dataConclusao: ''
      };
      this.editandoIndexFormacao = null;
    },

    resetarFormCertificacao() {
      this.novaCertificacao = {
        nome: '',
        instituicao: '',
        dataConclusao: '',
        cargaHoraria: '',
        urlCertificado: ''
      };
      this.editandoIndexCertificacao = null;
    },

    editarExperiencia(index) {
      const experiencia = this.curriculo.experiencias[index];

      const isEditingSameItem = this.editandoExperiencia && this.editandoIndexExperiencia === index;

      if (isEditingSameItem) {
            this.novaExperiencia = {
                cargo: '',
                empresa: '',
                dataInicio: '',
                dataFim: '',
                descricao: '',
                atual: false
            };
            this.editandoIndexExperiencia = null;
            this.editandoExperiencia = false;
            
      } else {
        this.novaExperiencia = {
         ...experiencia,
         dataInicio: experiencia.dataInicio?.split('T')[0] || '',
         dataFim: experiencia.dataFim?.split('T')[0] || ''
        };
        
        this.editandoIndexExperiencia = index;
        this.editandoExperiencia = true;
      }
    },

    editarFormacao(index) {
      const formacao = this.curriculo.formacoes[index];

      const isEditingSameItem = this.editandoFormacao && this.editandoIndexFormacao === index;
      
      if(isEditingSameItem){
        this.novaFormacao = {
          instituicao: '',
          curso: '',
          nivel: '',
          status: '',
          dataInicio: '',
          dataConclusao: '',
        }
        this.editandoIndexFormacao = null;
        this.editandoFormacao = null;
      }
      else{
        this.novaFormacao = {
          ...formacao,
          dataInicio: formacao.dataInicio?.split('T')[0] || '',
          dataConclusao: formacao.dataConclusao?.split('T')[0] || ''
        };
        
        this.editandoIndexFormacao = index;
        this.editandoFormacao = true;
      }
      
    },

    editarCertificacao(index) {
      const certificacao = this.curriculo.certificacoes[index];

      const isEditingSameItem = this.editandoCertificacao && this.editandoIndexCertificacao === index;
      
      if(isEditingSameItem){
        this.novaCertificacao = {
          nome: '',
          instituicao: '',
          dataConclusao: '',
          cargaHoraria: '',
          urlCertificado: '',
        }
        this.editandoIndexCertificacao = null;
        this.editandoCertificacao = null;
      }
      else{
        this.novaCertificacao = {
          ...certificacao,
          dataConclusao: certificacao.dataConclusao?.split('T')[0] || ''
        };
        
        this.editandoIndexCertificacao = index;
        this.editandoCertificacao = true;
      }
      
    },

    formatarTelefone(event) {
      let valor = event.target.value.replace(/\D/g, '');
      
      if (valor.length <= 11) {
        if (valor.length <= 10) {
          valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
          valor = valor.replace(/(\d)(\d{4})$/, '$1-$2');
        } else {
          valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
          valor = valor.replace(/(\d)(\d{4})$/, '$1-$2');
        }
      }
      
      this.telefoneFormatado = valor;
      this.curriculo.telefone = valor.replace(/\D/g, ''); 
    },

    formatarTelefoneString(telefone) {
      if (!telefone) return '';
      
      let valor = telefone.toString().replace(/\D/g, '');
      
      if (valor.length <= 11) {
        if (valor.length <= 10) {
          valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
          valor = valor.replace(/(\d)(\d{4})$/, '$1-$2');
        } else {
          valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
          valor = valor.replace(/(\d)(\d{4})$/, '$1-$2');
        }
      }
      
      return valor;
    },

    formatarPeriodo(dataInicio, dataFim, empregoAtual) {
      const formatarData = (dataString) => {
        if (!dataString) return 'Data não informada';
        
        const dataLimpa = dataString.split('T')[0];
        const data = new Date(dataLimpa + 'T00:00:00');
        
        if (isNaN(data.getTime())) return 'Data inválida';
        
        return data.toLocaleDateString('pt-BR', { day: 'numeric', month: 'numeric', year: 'numeric' });
      };

      const inicio = formatarData(dataInicio);
      let fim;
      
      if (empregoAtual) {
        fim = 'Atual';
      } else {
        fim = dataFim ? formatarData(dataFim) : 'Não Informado';
      }

      return `${inicio} - ${fim}`;
    },
    nextStep() {
      if (this.step < 4) this.step++;
    },
    
    prevStep() {
      if (this.step > 1) this.step--;
    },
    formatarDataAtual(dataRef) {
      const dataString = dataRef?.value || dataRef;
      
      if (!dataString) return '';
      
      const data = new Date(dataString);
      
      if (isNaN(data.getTime())) {
        console.error('Data inválida:', dataString);
        return '';
      }
      
      const dia = String(data.getDate()).padStart(2, '0');
      const mes = String(data.getMonth() + 1).padStart(2, '0');
      const ano = data.getFullYear();
      
      return `${dia}/${mes}/${ano}`;
    },
    async adicionarExperiencia() {
      if (!this.novaExperiencia.descricao) {
        return this.mostrarErro('Preencha a descrição da atividade.');
      }
      else if (!this.novaExperiencia.dataInicio) {
        return this.mostrarErro('Preencha a data de início da experiência.');
      }
      else if(!this.novaExperiencia.empregoAtual && this.novaExperiencia.dataFim == ''){
        return this.mostrarErro('Preencha a data fim da experiência.');
      }
      else if(!this.novaExperiencia.empregoAtual && (this.novaExperiencia.dataFim < this.novaExperiencia.dataInicio)){
        return this.mostrarErro('Data fim deve ser posterior à data de início.');
      }

      try {
          let novaExp;
          
          if (this.editandoIndexExperiencia !== null) { 
              const expId = this.curriculo.experiencias[this.editandoIndexExperiencia].id;
              
              if (expId) {
                  console.log(this.novaExperiencia);
                  await experienciaService.atualizarExperiencia(this.novaExperiencia);
              }
              
              this.curriculo.experiencias.splice(this.editandoIndexExperiencia, 1, { ...this.novaExperiencia });
              this.successMessage = 'Experiência atualizada!';

          } else {
              if (this.curriculoId) {
                  const experienciaComCurriculo = {
                      ...this.novaExperiencia,
                      dataFim: this.novaExperiencia.empregoAtual || !this.novaExperiencia.dataFim 
                          ? null 
                          : this.novaExperiencia.dataFim,
                      curriculoId: this.curriculoId
                  };
                  
                  novaExp = await experienciaService.adicionarExperiencia(experienciaComCurriculo);
                  
                  this.curriculo.experiencias.push(novaExp);
              } else {
                  this.curriculo.experiencias.push({ ...this.novaExperiencia });
              }
              
              this.successMessage = 'Experiência adicionada!';
          }
          
          this.resetarFormExperiencia(); 
          
      } catch (error) {
          const apiResponse = error.response;
          if (apiResponse && apiResponse.status === 400) {
              const errorCode = apiResponse.data.code;
              
              if (errorCode === 'DataInicio_Futura'){
                  return this.mostrarErro('A data de início não pode ser posterior à data atual.');
              } else {
                  return this.mostrarErro(apiResponse.data.message || 'Erro de validação ao salvar a experiência.');
              }
          } else {
              return this.mostrarErro('Erro ao salvar experiência. Verifique a conexão e o servidor.');
          }
          
      }
      
      setTimeout(() => this.successMessage = '', 2000);
    },
    removerExperiencia(index) {
      const experiencia = this.curriculo.experiencias[index];
      
      this.itemParaRemover = { 
        index, 
        tipo: 'experiencia',
        id: experiencia.id
      };
      
      this.modalConfig = {
        title: 'Excluir Experiência',
        message: 'Você tem certeza que deseja excluir esta experiência profissional?',
        type: 'perigo',
        confirmText: 'Sim, excluir'
      };
      
      this.showConfirmModal = true;
    },

    async adicionarFormacao() {
      if (!this.novaFormacao.instituicao) {
        return this.mostrarErro('Preencha a Nome da Instituição');
      }
      else if (!this.novaFormacao.nivel) {
        return this.mostrarErro('Preencha o Nível da formação');
      }
      else if (!this.novaFormacao.status) {
        return this.mostrarErro('Preencha o Status da formação');
      }
      else if (!this.novaFormacao.dataInicio) {
        return this.mostrarErro('Preencha a Data de Início');
      }
      else if (!this.novaFormacao.status) {
        return this.mostrarErro('Preencha a Data de Conclusão');
      }

      try {
        if (this.editandoIndexFormacao !== null) {
          const formId = this.curriculo.formacoes[this.editandoIndexFormacao].id;
          
          if (formId) {
            await formacaoService.atualizarFormacao(this.novaFormacao);
          }
          
          this.curriculo.formacoes.splice(this.editandoIndexFormacao, 1, { ...this.novaFormacao });
          this.successMessage = 'Formação atualizada!';

        } else {
          if (this.curriculoId) {
            const formacaoComCurriculo = {
              ...this.novaFormacao,
              curriculoId: this.curriculoId
            };
            const novaForm = await formacaoService.adicionarFormacao(formacaoComCurriculo);
            this.curriculo.formacoes.push(novaForm);
          } else {
            this.curriculo.formacoes.push({ ...this.novaFormacao });
          }
          
          this.successMessage = 'Formação adicionada!';
        }
        
        this.resetarFormFormacao();
        
      } catch (error) {
        const apiResponse = error.response; 

        if (apiResponse && apiResponse.status === 400) {
          const errorCode = apiResponse.data.code;
          
          if (errorCode === 'DataInicio_Futura') {
            return this.mostrarErro('A data de início não pode ser posterior à data atual.');
          } else {
            return this.mostrarErro('Erro de validação ao salvar a formação.');
          }
        } else {
          return this.mostrarErro('Erro ao salvar formação. Verifique a conexão e o servidor.');
        }
      }
      
      setTimeout(() => this.successMessage = '', 2000);
    },

    removerFormacao(index) {
      const formacao = this.curriculo.formacoes[index];
      
      this.itemParaRemover = { 
        index, 
        tipo: 'formacao',
        id: formacao.id
      };
      
      this.modalConfig = {
        title: 'Excluir Formação',
        message: 'Você tem certeza que deseja excluir esta formação acadêmica?',
        type: 'perigo',
        confirmText: 'Sim, excluir'
      };
      
      this.showConfirmModal = true;
    },

    async adicionarCertificacao() {
      if (!this.novaCertificacao.nome && !this.novaCertificacao.instituicao) {
        return this.mostrarErro('Preencha pelo menos o nome ou a instituição');
      }

      try {
        if (this.editandoIndexCertificacao !== null) {
          const certId = this.curriculo.certificacoes[this.editandoIndexCertificacao].id;
          
          if (certId) {
            await certificacaoService.atualizarCertificacao(this.novaCertificacao);
          }
          
          this.curriculo.certificacoes.splice(this.editandoIndexCertificacao, 1, { ...this.novaCertificacao });
          this.successMessage = 'Certificação atualizada!';

        } else {
          if (this.curriculoId) {
            const certificacaoComCurriculo = {
              ...this.novaCertificacao,
              curriculoId: this.curriculoId
            };
            const novaCert = await certificacaoService.adicionarCertificacao(certificacaoComCurriculo);
            this.curriculo.certificacoes.push(novaCert);
          } else {
            this.curriculo.certificacoes.push({ ...this.novaCertificacao });
          }
          
          this.successMessage = 'Certificação adicionada!';
        }
        
        this.resetarFormCertificacao();
        
      } catch (error) {
          const apiResponse = error.response; 

          if (apiResponse && apiResponse.status === 400) {
              const errorCode = apiResponse.data.message;
              if (errorCode === 'DataConclusao_Futura'){
                  return this.mostrarErro('A data de início não pode ser posterior à data atual.');
              } else {
                  return this.mostrarErro(apiResponse.data.message || 'Erro de validação ao salvar o certificado.');
              }
          } else {
              return this.mostrarErro('Erro ao salvar certificado. Verifique a conexão e o servidor.');
          }
          
      }
      setTimeout(() => this.successMessage = '', 2000);
    },

    removerCertificacao(index) {
      const certificacao = this.curriculo.certificacoes[index];
      
      this.itemParaRemover = { 
        index, 
        tipo: 'certificacao',
        id: certificacao.id
      };
      
      this.modalConfig = {
        title: 'Excluir Certificação',
        message: 'Você tem certeza que deseja excluir esta certificação?',
        type: 'perigo',
        confirmText: 'Sim, excluir'
      };
      
      this.showConfirmModal = true;
    },

    async confirmarRemocao() {
      if (!this.itemParaRemover) return;

      const { index, tipo, id } = this.itemParaRemover;

      try {
        switch (tipo) {
          case 'experiencia':
            if (id) {
              await experienciaService.excluirExperiencia(id);
            }
            this.curriculo.experiencias.splice(index, 1);
            
            if (this.editandoIndexExperiencia === index) {
              this.resetarFormExperiencia();
            } else if (this.editandoIndexExperiencia !== null && index < this.editandoIndexExperiencia) {
              this.editandoIndexExperiencia--;
            }
            
            this.successMessage = 'Experiência removida!';
            break;
            
          case 'formacao':
            if (id) {
              await formacaoService.excluirFormacao(id);
            }
            this.curriculo.formacoes.splice(index, 1);
            
            if (this.editandoIndexFormacao === index) {
              this.resetarFormFormacao();
            } else if (this.editandoIndexFormacao !== null && index < this.editandoIndexFormacao) {
              this.editandoIndexFormacao--;
            }
            
            this.successMessage = 'Formação removida!';
            break;
            
          case 'certificacao':
            if (id) {
              await certificacaoService.excluirCertificacao(id);
            }
            this.curriculo.certificacoes.splice(index, 1);
            
            if (this.editandoIndexCertificacao === index) {
              this.resetarFormCertificacao();
            } else if (this.editandoIndexCertificacao !== null && index < this.editandoIndexCertificacao) {
              this.editandoIndexCertificacao--;
            }
            
            this.successMessage = 'Certificação removida!';
            break;
        }

        setTimeout(() => this.successMessage = '', 2000);
      } catch (error) {
        console.error('Erro ao excluir:', error);
        return this.mostrarErro(`Erro ao excluir ${tipo}. Tente novamente.`);
      } finally {
        this.itemParaRemover = null;
        this.showConfirmModal = false;
      }
    },

    async formatarComIA() {
      if (!this.novaExperiencia.descricao || this.novaExperiencia.descricao.trim().length < 10) {
        this.iaMessage = 'Digite pelo menos 10 caracteres';
        this.iaMessageType = 'info';
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);
        return;
      }

      this.loadingIA = true;
      this.iaMessage = 'Melhorando descrição...';
      this.iaMessageType = 'info';

      try {
        const iaFormattingService = (await import('@/services/iaFormattingService')).default;
        
        const descricaoMelhorada = await iaFormattingService.formatarDescricaoProfissional(
          this.novaExperiencia.descricao,
          this.novaExperiencia.cargo,
          this.novaExperiencia.empresa
        );
        
        this.novaExperiencia.descricao = descricaoMelhorada;
        
        this.iaMessage = 'Descrição melhorada!';
        this.iaMessageType = 'success';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);

      } catch (error) {
        console.error('Erro ao formatar com IA:', error);
        this.iaMessage = 'Erro ao melhorar. Tente novamente.';
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
      try {
        this.loading = true;
        
        if (this.modoEdicao) {
          await curriculoService.atualizarCurriculo(this.curriculo);
          this.successMessage = 'Currículo atualizado com sucesso!';
        } else {
          const data = await curriculoService.adicionarCurriculo(this.curriculo);
          this.curriculoId = data.id;
          this.successMessage = 'Currículo salvo com sucesso!';
        }
        
        setTimeout(() => {
          this.$router.push(`/curriculo/visualizar/${this.curriculo.id || this.curriculoId}`);
        }, 2000);
      } catch (error) {
        return this.mostrarErro('Erro ao salvar currículo');
      } finally {
        this.loading = false;
      }
    },
    async continuarPerfil() {
    try {
        this.loading = true;
        
        if (this.modoEdicao) {
            await curriculoService.atualizarCurriculo(this.curriculo);
            this.successMessage = 'Currículo atualizado com sucesso!';
            setTimeout(() => this.successMessage = '', 3000);
        } else {
            const data = await curriculoService.adicionarCurriculo(this.curriculo);
            this.curriculoId = data.id;
            this.curriculo.id = data.id;
            this.successMessage = 'Currículo salvo com sucesso!';
            setTimeout(() => this.successMessage = '', 3000);
        }
        this.curriculoOriginal = JSON.parse(JSON.stringify(this.curriculo));
        
        return true;
    } catch (error) {
        const apiResponse = error.response;
        if (apiResponse && apiResponse.status === 400) {
            const errorCode = apiResponse.data.code;
            if (errorCode === 'DataNascimento_Invalida'){
                return this.mostrarErro('A data de nascimento é inválida');
            } else {
                return this.mostrarErro('Erro de validação ao salvar a experiência.');
            }
        } else {
            return this.mostrarErro('Erro ao salvar experiência. Verifique a conexão e o servidor.');
        }
      } finally {
        this.loading = false;
      }
  },
  nextStepPerfil() {
    if (this.hasChanges()) {
        this.continuarPerfil().then(sucesso => {
            if (sucesso) {
                this.step++;
                console.log("AA")
            }
        });
    } else {
        this.step++;
        console.log("AA")
    }
  },
  async voltarLogin() {
    try {
      await usuarioService.logout();
      
      await new Promise(resolve => setTimeout(resolve, 100));
      
      this.$router.replace('/login').then(() => {
        window.location.reload();
      });
      
    } catch (error) {
      console.error('Erro ao fazer logout:', error);
      this.$router.replace('/login').then(() => {
        window.location.reload();
      });
    }
  },
  abrirModal() {
      this.modalAberto = true;
    },
    fecharModal() {
      this.modalAberto = false;
    },
    async confirmarSair() {
      try {
        await usuarioService.logout();
        this.curriculo = null;
        this.$router.replace('/login').then(() => {
          window.location.reload();
        });
      } catch (error) {
        console.error('Erro ao fazer logout:', error);
        this.$router.replace('/login').then(() => {
          window.location.reload();
        });
      }
    },
    hasChanges() {
      return JSON.stringify(this.curriculo) !== JSON.stringify(this.curriculoOriginal);
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
  display: flex;
  align-items: center;
  justify-content: center;
  background: #fafafa;
  padding: 40px 24px;
}

.container {
  background: white;
  border-radius: 16px;
  border: 1px solid #e5e7eb;
  padding: 48px;
  width: 100%;
  max-width: 720px;
  animation: fadeIn 0.4s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(8px);
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
  width: 56px;
  height: 56px;
  background: #000;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 20px;
  color: white;
}

.header h1 {
  font-size: 24px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 8px;
  letter-spacing: -0.02em;
}

.header p {
  font-size: 14px;
  color: #6b7280;
}

.progress-bar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 40px;
}

.progress-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
}

.step-circle {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: #f3f4f6;
  color: #9ca3af;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  font-size: 14px;
  transition: all 0.2s ease;
}

.progress-step.active .step-circle {
  background: #000;
  color: white;
}

.progress-step span {
  font-size: 11px;
  color: #9ca3af;
  font-weight: 500;
}

.progress-step.active span {
  color: #111827;
  font-weight: 600;
}

.progress-line {
  flex: 1;
  height: 2px;
  background: #e5e7eb;
  margin: 0 8px;
  margin-bottom: 28px;
  transition: all 0.3s ease;
}

.progress-line.active {
  background: #000;
}

.step-content {
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(8px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.step-title {
  font-size: 20px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 24px;
  letter-spacing: -0.01em;
}

.form-card {
  background: #fafafa;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 24px;
  margin-bottom: 24px;
}

.form-group {
  margin-bottom: 18px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

label {
  display: block;
  font-size: 13px;
  font-weight: 500;
  color: #374151;
  margin-bottom: 8px;
}

input, select, textarea {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 14px;
  transition: all 0.2s ease;
  background: white;
  color: #111827;
  font-family: inherit;
}

textarea {
  resize: vertical;
  min-height: 80px;
}

input:focus, select:focus, textarea:focus {
  outline: none;
  border-color: #000;
  box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.05);
}

input::placeholder, textarea::placeholder {
  color: #9ca3af;
}

.checkbox-wrapper {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-top: 10px;
}

.checkbox-wrapper input[type="checkbox"] {
  width: 18px;
  height: 18px;
  cursor: pointer;
}

.checkbox-wrapper label {
  margin: 0;
  cursor: pointer;
  font-size: 13px;
}

.btn-primary {
  width: 100%;
  padding: 12px;
  background: #000;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 44px;
}

.btn-primary:hover:not(:disabled) {
  background: #1f2937;
}

.btn-primary:active:not(:disabled) {
  transform: scale(0.98);
}

.btn-primary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-secondary {
  padding: 12px;
  background: white;
  color: #111827;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-secondary:hover {
  background: #fafafa;
}

.btn-add {
  width: 100%;
  padding: 10px;
  background: #000;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  transition: all 0.2s ease;
}

.btn-add:hover {
  background: #1f2937;
}

.btn-back{
  padding: 10px 20px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  background: white;
  color: #111827;
  border: 1px solid #e5e7eb;
  left: auto;
}

.btn-back:hover {
  background: #f9fafb;
}

.btn-ia {
  width: 100%;
  margin-top: 10px;
  padding: 10px;
  background: #6366f1;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.btn-ia:hover:not(:disabled) {
  background: #4f46e5;
}

.btn-ia:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.loading-spinner-small {
  width: 16px;
  height: 16px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

.ia-message {
  display: block;
  margin-top: 10px;
  font-size: 12px;
  font-weight: 500;
  padding: 8px 12px;
  border-radius: 6px;
}

.ia-message.success {
  background: #f0fdf4;
  color: #166534;
}

.ia-message.error {
  background: #fef2f2;
  color: #991b1b;
}

.ia-message.info {
  background: #eff6ff;
  color: #1e40af;
}

.button-group {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
  margin-top: 24px;
}

.item-list {
  margin-top: 20px;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.item-card {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  padding: 16px;
  position: relative;
  animation: fadeIn 0.3s ease;
}

.item-card h4 {
  color: #111827;
  font-size: 15px;
  font-weight: 600;
  margin-bottom: 6px;
  padding-right: 70px;
}

.item-card .subtitle {
  color: #6b7280;
  font-size: 14px;
  margin-bottom: 4px;
}

.item-card .date {
  color: #9ca3af;
  font-size: 13px;
  margin-bottom: 8px;
}

.item-card .description {
  color: #6b7280;
  font-size: 13px;
  line-height: 1.5;
}

.btn-remove {
  position: absolute;
  top: 12px;
  right: 12px;
  width: 28px;
  height: 28px;
  background: #fef2f2;
  color: #991b1b;
  border: none;
  border-radius: 6px;
  font-size: 20px;
  line-height: 1;
  cursor: pointer;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-edit {
  position: absolute;
  top: 12px;
  right: 50px;
  width: 28px;
  height: 28px;
  background: #f9f9f9;
  color: #000000;
  border: none;
  border-radius: 6px;
  font-size: 12px;
  line-height: 1;
  cursor: pointer;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-remove:hover {
  background: #fecaca;
}

.btn-edit:hover {
  background: #e5e7eb;
}

.loading-spinner {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.alert {
  padding: 12px 14px;
  border-radius: 8px;
  font-size: 14px;
  margin-top: 16px;
  animation: slideDown 0.3s ease;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-4px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.alert-success {
  background: #f0fdf4;
  color: #166534;
  border: 1px solid #bbf7d0;
}

.alert-error {
  background: #fef2f2;
  color: #991b1b;
  border: 1px solid #fecaca;
}

@media (max-width: 768px) {
  .container {
    padding: 32px 24px;
  }

  .header h1 {
    font-size: 22px;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .button-group {
    grid-template-columns: 1fr;
  }

  .progress-bar {
    padding: 0;
  }

  .progress-step span {
    font-size: 10px;
  }

  .step-circle {
    width: 32px;
    height: 32px;
    font-size: 13px;
  }
}
</style>