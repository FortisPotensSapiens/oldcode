import { Card } from 'antd';

import { MAP_STATUS_NAME } from '~/pages/PartnerProducts/utils';
import { MerchandiseReadModel } from '~/types';

const AdminProductsCellsExtended = ({ record }: { record: MerchandiseReadModel }) => {
  return (
    <Card>
      <Card.Meta
        description={
          <>
            <div>
              <strong>Описание</strong>
              <p>{record.description}</p>
            </div>
            <div>
              <strong>Продавец</strong>
              <p>{record.seller.title}</p>
            </div>
            <div>
              <strong>Теги</strong>
              <p>
                {record.categories?.map((item, index) => {
                  return `${index !== 0 ? ', ' : ''}${item.title} `;
                })}
              </p>
            </div>
            <div>
              <strong>Статус</strong>
              <p>{MAP_STATUS_NAME[record.state]}</p>
            </div>
          </>
        }
        title={record.title}
      />
    </Card>
  );
};

export default AdminProductsCellsExtended;
