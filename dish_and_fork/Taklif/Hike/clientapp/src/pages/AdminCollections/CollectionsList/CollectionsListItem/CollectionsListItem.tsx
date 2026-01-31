import { DeleteFilled, EditFilled } from '@ant-design/icons';
import styled from '@emotion/styled';
import { Button, Card, Popconfirm, Tag } from 'antd';
import { FC } from 'react';

import { CollectionReadModel } from '~/types/collection';

const StyledTag = styled(Tag)`
  margin-bottom: 1rem;
  font-size: 120%;
`;

const StyledButton = styled(Button)`
  margin-left: 0.3rem;
`;

const CollectionsListItem: FC<{
  collection: CollectionReadModel;
  onDeleteCollection: (id: string) => void;
  onEditCollection: (collection: CollectionReadModel) => void;
}> = ({ collection, onDeleteCollection, onEditCollection }) => {
  const onDelete = () => {
    onDeleteCollection(collection.id);
  };

  const onEdit = () => {
    onEditCollection(collection);
  };

  return (
    <Card
      title={collection.title}
      extra={
        <>
          <StyledButton onClick={onEdit}>
            <EditFilled />
          </StyledButton>
          <Popconfirm title="Вы уверены что хотите удалить коллекцию" onConfirm={onDelete} okText="Да" cancelText="Нет">
            <StyledButton>
              <DeleteFilled />
            </StyledButton>
          </Popconfirm>
        </>
      }
      style={{ width: 400 }}
    >
      {collection.categories?.map((category) => {
        return <StyledTag key={category.id}>{category.title}</StyledTag>;
      })}
    </Card>
  );
};

export { CollectionsListItem };
