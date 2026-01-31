import { FileReadModel } from '~/types';

export type ChangeHandler = (value: ImageValue[]) => void;

export const ItemTypes = {
  IMAGE: 'image',
};

export type ImageValue = string | FileReadModel;
