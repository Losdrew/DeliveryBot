export interface ErrorDto {
  validationErrors?: ValidationErrorDto[] | undefined;
  serviceErrors?: ServiceErrorDto[] | undefined;
}

export interface ServiceErrorDto {
  header?: string | undefined;
  errorMessage?: string | undefined;
  code?: number;
}

export interface ValidationErrorDto {
  fieldCode?: string | undefined;
  errorMessage?: string | undefined;
}