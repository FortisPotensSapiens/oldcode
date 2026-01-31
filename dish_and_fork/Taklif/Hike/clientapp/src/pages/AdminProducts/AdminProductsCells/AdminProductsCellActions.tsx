import { Button, List } from 'antd';

import { MerchandiseReadModel } from '~/types';

const AdminProductsCellActions = ({
  record,
  onEdit,
  onApprove,
}: {
  record: MerchandiseReadModel;
  onEdit: (id: string) => void;
  onApprove: (record: MerchandiseReadModel) => void;
}) => {
  const onEditCallback = () => {
    onEdit(record.id);
  };

  const onTagsApprove = () => {
    onApprove(record);
  };

  return (
    <List>
      {!record.isTagsAppovedByAdmin ? (
        <List.Item>
          <Button onClick={onTagsApprove}>Подтвердить теги</Button>
        </List.Item>
      ) : undefined}
      <List.Item>
        <Button type="primary" onClick={onEditCallback}>
          Редактировать
        </Button>
      </List.Item>
    </List>
  );
};

export default AdminProductsCellActions;
