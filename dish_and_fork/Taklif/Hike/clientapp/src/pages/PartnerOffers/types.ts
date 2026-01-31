import { OfferSellerReadModel } from '~/types/swagger';

export type ColumnProps = { row: OfferSellerReadModel; onDelete: (offerId: string) => void };
