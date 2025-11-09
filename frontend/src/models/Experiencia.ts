export interface Experiencia {
  id: string;
  curriculoId: string; 
  empresa: string | null;
  cargo: string | null;
  dataInicio: string;
  dataFim: string | null;
  empregoAtual: boolean;
  descricao: string | null;
  excluido: boolean | null;
}