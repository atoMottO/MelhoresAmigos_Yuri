export interface Formacao {
  id: string;
  curriculoId: string; 
  instituicao: string | null;
  curso: string | null;
  nivel: string | null;
  status: string | null;
  dataInicio: string | null;
  dataConclusao: string | null;
  excluido: boolean | null;
}