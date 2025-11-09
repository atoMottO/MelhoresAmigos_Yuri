export interface Usuario {
  id: string; 
  nome: string | null;
  email: string | null;
  senha: string | null;
  ativo: boolean | null;
  excluido: boolean | null;
}