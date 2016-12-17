import Button from '../app/components/Button';
import ImagePair from './ImagePair.react';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';
import { fetchReferences, deleteAllReferences } from '../../common/api/actions';
import { selectReference } from '../../common/app/actions';
import { FileUpload } from 'redux-file-upload';

@connect(state => ({
  references: state.api.getIn(['references', 'data']),
  referencesPendingDelete: state.api.getIn(['references', 'pendingDelete']),
  selectedReference: state.app.get('selectedReference')
}), { fetchReferences, deleteAllReferences, selectReference })
export default class References extends Component {

  static propTypes = {
    deleteAllReferences: RPT.func,
    fetchReferences: RPT.func.isRequired,
    references: RPT.array,
    referencesPendingDelete: RPT.bool,
    selectReference: RPT.func,
    selectedReference: RPT.string
  }

  componentDidMount() {
    const { fetchReferences } = this.props;
    fetchReferences();
  }

  renderReference(key, reference) {
    const { selectedReference, selectReference } = this.props;
    const { imageUrl, contourImageUrl, cannyTreshodLinking, cannyTreshold } = reference.toJS();

    return (
      <ImagePair
        cannyTreshodLinking={cannyTreshodLinking}
        cannyTreshold={cannyTreshold}
        contourUrl={contourImageUrl}
        id={key}
        imageUrl={imageUrl}
        isSelected={selectedReference === key}
        onClick={() => selectReference(key)}
        pairType="reference"
      />
    );
  }

  render() {
    const { references, referencesPendingDelete, deleteAllReferences } = this.props;

    return (
      <div>
        <div>
          <h1>Referenční obrázky</h1>
          <div style={styles.imagesWrapper}>
            {references && references.map((x, key) => this.renderReference(key, x))}
          </div>
          {references && references.size > 0 && <Button backgroundColor="secondary" disabled={referencesPendingDelete} onClick={() => deleteAllReferences()}>Smazat vše</Button>}
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg', 'png']}
            data={{ type: 'picture' }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/reference"
          >
            <Button>
              Klikněte nebo přetáhněte referenční obrázky (JPG, PNG)
            </Button>
          </FileUpload>
        </div>
      </div>
    );
  }
}

const styles = {
  imagesWrapper: {
    overflow: 'auto',
    height: 'auto',
    clear: 'both'
  }
};
