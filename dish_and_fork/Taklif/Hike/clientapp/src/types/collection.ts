import { CategoryReadModel } from './swagger';

export interface CollectionReadModel {
  id: string;
  title: string;
  categories: CategoryReadModel[];
}
