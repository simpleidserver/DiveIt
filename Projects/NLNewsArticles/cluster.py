from sklearn.cluster import MiniBatchKMeans
from sklearn.metrics.pairwise import pairwise_distances_argmin
from sklearn.metrics import silhouette_samples

def mbkmeans_clusters(
	X,
    k,
    mb
):
    mbk = MiniBatchKMeans(n_clusters=k, batch_size=mb)
    mbk.fit(X)
    ## mbk_means_cluster_centers = np.sort(mbk.cluster_centers_, axis=0)
    ## mbk_means_labels = pairwise_distances_argmin(X, mbk_means_cluster_centers)

    return mbk, mbk.labels_